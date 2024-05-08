--creation of tables.


--OWNER
CREATE TABLE OWNER(
	oID VARCHAR(7) PRIMARY KEY NOT NULL,
	NAME VARCHAR(50) NOT NULL,
	--gID VARCHAR(7), --owner will not store the gym id he or she is responsible for but the gym will track on what basis the gym relates to the owner.
	REG_INFO VARCHAR(100),
	--FOREIGN KEY(gID) REFERENCES GYM(gID)
	);
--OWNER

--GYM
CREATE TABLE GYM(
	gID VARCHAR(7) PRIMARY KEY NOT NULL,
	NAME VARCHAR(50) NOT NULL,
	RATE INTEGER,
	FINANCE VARCHAR(100),
	oID VARCHAR(7) NOT NULL,
	MEMBERS INT,
	LOCATION VARCHAR(30) NOT NULL,
	FOREIGN KEY(oID) REFERENCES OWNER(oID)
	);
--GYM

--ADMIN
CREATE TABLE ADMIN(
	aID VARCHAR(7) PRIMARY KEY NOT NULL,
	NAME VARCHAR(50) NOT NULL,
	PASS VARCHAR(20) UNIQUE NOT NULL,
	DESCRIPTION VARCHAR(100)
	);
--ADMIN

--TRAINER
CREATE TABLE TRAINER(
	tID VARCHAR(7) PRIMARY KEY NOT NULL,
	NAME VARCHAR(30) NOT NULL,
	gID VARCHAR(7) NOT NULL,
	--speciality table that stores the speciality based on the trainer id
	EXPERIENCE VARCHAR(20) NOT NULL, --trainer should enter atleast a description of experience in years etc.
	CLIENTS INT, --can be null will be calculated according to number of training sessions under a specific trainer
	FOREIGN KEY(gID) REFERENCES GYM(gID)
	);

--TRAINER



--MEMBER



--MEMBER


--WORK PLAN


--WORK PLAN


--DIET PLAN


--DIET PLAN


--REQUESTS



--REQUESTS



--REVIEWS


--REVIEWS


--TRAINING SESSION

--TRAINING SESSION

