--Queries

--creating a incrementing value sequence for the audit IDs.
CREATE SEQUENCE audit_id_seq START WITH 4 INCREMENT BY 1;
CREATE SEQUENCE audit_acc START WITH 104 INCREMENT BY 1;
CREATE SEQUENCE tran_id START WITH 4 INCREMENT BY 1;

CREATE TRIGGER Insert_Customer
ON Customer
AFTER INSERT
AS
BEGIN

	DECLARE @new_id INT;
	DECLARE @new_acc INT;
	DECLARE @new_time DATETIME;
	SET @new_id = NEXT VALUE FOR audit_id_seq;
	SET @new_acc = NEXT VALUE FOR audit_acc;
	SELECT @new_time = GETDATE()
	FROM inserted;
	INSERT INTO Audit_Log(
		auditID,
		UserID,
		action,
		time,
		details
		)
	VALUES(
		@new_id,
		67, --since the value of last 2 digits of the roll num were needed, did not take them from the accounts table as it is required to add in the customer table.
		'Create Account',
		@new_time,
		CONCAT('Account ',@new_acc,' created') ----slight mistake where the added id is 104 but if we use 67 as the number here instead of accnum the value would be 67.
		);
END;
--update trigger on the account table.
CREATE TRIGGER insert_transact
ON Activity
AFTER INSERT
AS 
BEGIN
	DECLARE @new_id INT;
	DECLARE @new_time DATETIME;
	DECLARE @acc_id INT;
	DECLARE @tId INT;
	SET @new_id = NEXT VALUE FOR audit_id_seq;
	SET @tID = NEXT VALUE FOR tran_id;
	SELECT @acc_id = inserted.accountID ,@new_time = GETDATE() FROM inserted;
	INSERT INTO Audit_Log(
		auditID,
		UserID,
		action,
		time,
		details
		)
	VALUES(
		@new_id,
		@acc_id,
		'inserted transaction',
		@new_time,
		CONCAT('Transaction ',@tID,' added') 
		);
END;

CREATE TRIGGER delete_transact
ON Activity
AFTER DELETE
AS 
BEGIN
	DECLARE @new_id INT;
	DECLARE @new_time DATETIME;
	DECLARE @acc_id INT;
	DECLARE @tId INT;
	SET @new_id = NEXT VALUE FOR audit_id_seq;
	SELECT @acc_id = deleted.accountID ,@new_time = GETDATE(),@tID = deleted.transactionID FROM deleted;
	INSERT INTO Audit_Log(
		auditID,
		UserID,
		action,
		time,
		details
		)
	VALUES(
		@new_id,
		@acc_id,
		'deleted transaction',
		@new_time,
		CONCAT('Transaction ',@tID,' deleted') 
		);
END;

CREATE TRIGGER acc_reset
ON Account
AFTER UPDATE
AS
BEGIN
	DECLARE @new_id INT;
	DECLARE @new_time DATETIME;
	DECLARE @acc_id INT;
	SET @new_id = NEXT VALUE FOR audit_id_seq;
	SELECT @acc_id = deleted.accountID ,@new_time = GETDATE() FROM deleted;
	INSERT INTO Audit_Log(
		auditID,
		UserID,
		action,
		time,
		details
		)
	VALUES(
		@new_id,
		@acc_id,
		'updated user account',
		@new_time,
		CONCAT('Account ',@acc_id,' updated') 
		);
END;


--operations
INSERT INTO Customer VALUES(4,'Ibtehaj','i220767@nu.edu.pk',1234567,'FAST ISB');
INSERT INTO Account VALUES (67,4,'Current',10000);

--transations performed by the customer which is us.
--1
INSERT INTO Activity VALUES (4,67,'withdrawal',2000,GETDATE());
--2
INSERT INTO Activity VALUES (5,67,'deposit',3000,GETDATE());
--3
INSERT INTO Activity VALUES (6,67,'deposit',2000,GETDATE());
--4
DELETE FROM Activity WHERE transactionID = 6;

--update for user
UPDATE Account
SET balance = 13000 WHERE accountID = 67; 
SELECT * FROM Audit_Log

--last query for user transactions
SELECT A.accountID,RES.name,A.AccType,AV.TransactionType,AV.amount,AV.time
FROM (SELECT customerID,name FROM Customer WHERE name = 'Ibtehaj') as RES JOIN Account A ON RES.customerID = A.customerID JOIN Activity AV ON A.accountID = AV.accountID
