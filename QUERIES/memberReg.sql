CREATE PROCEDURE RegisterMemberDetails
	@usrnem VARCHAR(30),
	@email VARCHAR(100),
	@pass VARCHAR(100),
	@type VARCHAR(50),
	@gname VARCHAR(100)

AS
BEGIN
	DECLARE @id INT;
	DECLARE @gID VARCHAR(7);
	DECLARE @expiry DATETIME;
	DECLARE @mbID INT;
	--performing 2 insertions in one.
	SET @id = (SELECT COUNT(*) + 1 FROM MEMBER);
	SET @gID = (SELECT gym_id FROM GYM WHERE name = @gname);
	SET @expiry = (SELECT DATEADD(year,1,GETDATE()));
	SET @mbID = (SELECT COUNT(*) + 1 FROM Membership);
	INSERT INTO Membership(membership_id,gym_id,expiry,type) VALUES(CONCAT('P',@mbID),@gID,@expiry,@type);
	INSERT INTO MEMBER(member_id,membership_id,workoutPlan_id,dietPlan_id,name,weight,email,password,verified) VALUES (CONCAT('M' , @id),CONCAT('P',@mbID),'NULL','NULL',@usrnem,0,@email,@pass,0);
END
DROP PROCEDURE RegisterMemberDetails
CALL RegisterMemberDetails('Ibtehaj','ibtehaj@gmail.com','password','Student','Gym Revolution') ;
EXEC RegisterMemberDetails @usrnem = 'ibtehaj',@email='ibtehaj@gmail.com',@pass='password',@type='Student',@gname='Gym Revolution';
