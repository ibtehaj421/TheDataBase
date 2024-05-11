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


--add diet plan change according to requirements

CREATE PROCEDURE AddDietPlan(
	@memberID VARCHAR(7),
	@nutri1 VARCHAR(30),
	@nutri2 VARCHAR(30),
	@type VARCHAR(30),
	@objective VARCHAR(30),
	@difficulty VARCHAR(30),
	@guideline VARCHAR(100)
	)
AS 
BEGIN
	DECLARE @nutriID VARCHAR(7);
	DECLARE @nutriID2 VARCHAR(7);
	DECLARE @dietID INT;
	DECLARE @mealID INT;
	DECLARE @nutriCal INT;
	DECLARE @nutriCal2 INT;
	SET @nutriID = (SELECT nutrition_id FROM NUTRITION WHERE name = @nutri1);
	SET @nutriID2 = (SELECT nutrition_id FROM NUTRITION WHERE name = @nutri2);
	SET @dietID = (SELECT COUNT(*)+1 FROM DIET_PLAN);
	SET @mealID = (SELECT COUNT(*)+1 FROM MEAL);
	SET @nutriCal = (SELECT calories FROM NUTRITION WHERE name = @nutri1);
	SET @nutriCal2 = (SELECT calories FROM NUTRITION WHERE name = @nutri2);
	INSERT INTO MEAL (meal_id,nutrition_id) VALUES (CONCAT('ML',@mealID),@nutriID);
	INSERT INTO meal_nutrient(nutrient_id,meal_id,calorie) VALUES (@nutriID,CONCAT('ML',@mealID),@nutriCal);
	INSERT INTO meal_nutrient(nutrient_id,meal_id,calorie) VALUES (@nutriID2,CONCAT('ML',@mealID),@nutriCal2);
	INSERT INTO DIET_PLAN(dietPlan_id,meal_id,type,objective,guidelines,difficulty_level) VALUES (CONCAT('D',@dietID),CONCAT('ML',@mealID),@type,@objective,@guideline,@difficulty);
	INSERT INTO MEMBER_DIETS(member_id,diet_id) VALUES (@memberID,CONCAT('D',@dietID));
END
DROP PROCEDURE AddDietPlan

EXEC AddDietPlan @memberID='M1',@nutri1='Omega-3' ,@nutri2='Creatine' ,@type='Lunch' ,@objective='General Health' ,@difficulty='Easy' ,@guideline='IDK man use if you want to' ;
