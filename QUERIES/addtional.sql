
CREATE TABLE MEMBER_DIETS(
	member_id VARCHAR(7) NOT NULL,
	diet_id VARCHAR(7) NOT NULL,
	PRIMARY KEY(member_id,diet_id),
	FOREIGN KEY(member_id) REFERENCES MEMBER(member_id),
	FOREIGN KEY(diet_id) REFERENCES DIET_PLAN(dietPlan_id)
	);
SELECT * FROM MEMBER_DIETS
BULK INSERT meal_nutrient
FROM 'C:\Users\PC\Downloads\Meal_Nutrient.csv'
WITH(
	FORMAT = 'CSV',
	FIRSTROW = 2
);
DELETE FROM MEMBER_DIETS
DELETE FROM MEMBER
DELETE FROM DIET_PLAN
WITH SpecificDiet AS(
	SELECT type,objective,meal_id FROM DIET_PLAN WHERE dietPlan_id = 'D10'
	),
	MealCals AS(
	SELECT meal_id,SUM(calorie) as cals FROM meal_nutrient GROUP BY meal_id
	)
SELECT SD.type,SD.objective,MC.cals FROM SpecificDiet SD JOIN MealCals MC ON SD.meal_id = MC.meal_id;



CREATE TABLE TRAINING_SESSION
(
	trainingSession_id VARCHAR(7) PRIMARY KEY NOT NULL,
	trainer_id VARCHAR(7) NOT NULL,
	gym_id VARCHAR(7) NOT NULL,
	member_id VARCHAR(7) NOT NULL,
	[date] DATE,
	
	FOREIGN KEY(trainer_id) REFERENCES TRAINER(trainer_id),
	FOREIGN KEY(member_id) REFERENCES [MEMBER](member_id),
	FOREIGN KEY(gym_id) REFERENCES GYM(gym_id)
);

WITH TrainerGym AS(
	SELECT trainer_id,gym_id FROM TRAINING_SESSION WHERE member_id = 'M16')
	SELECT T.name,G.name,T.experience FROM TRAINER T JOIN TrainerGym TG ON T.trainer_id = TG.trainer_id JOIN GYM G ON G.gym_id = TG.gym_id;


--all diet plan related queries.
--show all diet plans with their meals nutrition calorie and allergen(if any)

WITH dietReport AS(
	SELECT dietPlan_id as diet_id,meal_id,type FROM DIET_PLAN),
	extractAllergen AS(
	SELECT N.nutrition_id,N.name as nutrition,A.name as allergen FROM NUTRITION N JOIN ALLERGEN  A ON N.allergen_id = A.allergen_id)
	SELECT diet_id,type,nutrition,calorie,allergen FROM dietReport DP JOIN meal_nutrient MN ON DP.meal_id = MN.meal_id JOIN extractAllergen EA ON MN.nutrient_id = EA.nutrition_id ORDER BY diet_id;
	
--show all diet plans with their meals nutrition calorie and allergen

SELECT * FROM NUTRITION

WITH dietReport AS(
	SELECT M.member_id,D.dietPlan_id as diet_id,meal_id,type FROM DIET_PLAN D JOIN MEMBER_DIETS M ON D.dietPlan_id = M.diet_id),
	extractAllergen AS(
	SELECT N.nutrition_id,N.name as nutrition,A.name as allergen FROM NUTRITION N JOIN ALLERGEN  A ON N.allergen_id = A.allergen_id)
	SELECT member_id,diet_id,type,nutrition,calorie,allergen FROM dietReport DP JOIN meal_nutrient MN ON DP.meal_id = MN.meal_id JOIN extractAllergen EA ON MN.nutrient_id = EA.nutrition_id ORDER BY diet_id;

	
--specific member diet plan search 
WITH dietReport AS(
	SELECT dietPlan_id as diet_id,meal_id,type FROM DIET_PLAN D JOIN MEMBER_DIETS M ON M.diet_id = D.dietPlan_id WHERE member_id = 'M16'),
	extractAllergen AS(
	SELECT N.nutrition_id,N.name as nutrition,A.name as allergen FROM NUTRITION N JOIN ALLERGEN  A ON N.allergen_id = A.allergen_id)
	SELECT diet_id,type,nutrition,calorie,allergen FROM dietReport DP JOIN meal_nutrient MN ON DP.meal_id = MN.meal_id JOIN extractAllergen EA ON MN.nutrient_id = EA.nutrition_id ORDER BY diet_id;


--show all trainer plans.
WITH dietReport AS(
	SELECT M.trainer_id,M.dietPlan_id as diet_id,meal_id,type FROM DIET_PLAN D JOIN TRAINER_DIETPLAN M ON M.dietPlan_id = D.dietPlan_id),
	extractAllergen AS(
	SELECT N.nutrition_id,N.name as nutrition,A.name as allergen FROM NUTRITION N JOIN ALLERGEN  A ON N.allergen_id = A.allergen_id)
	SELECT trainer_id,diet_id,type,nutrition,calorie,allergen FROM dietReport DP JOIN meal_nutrient MN ON DP.meal_id = MN.meal_id JOIN extractAllergen EA ON MN.nutrient_id = EA.nutrition_id ORDER BY trainer_id;

SELECT * FROM TRAINER_DIETPLAN

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

SELECT * FROM DIET_PLAN
SELECT * FROM MEMBER
SELECT * FROM NUTRITION;
DROP TABLE meal_nutrient
SELECT * FROM meal_nutrient
CREATE TABLE meal_nutrient(
	--consists of meal and nutrient id as a meal can have many nutrient and many nutrients can be found in many meals.
	nutrient_id VARCHAR(7) NOT NULL,
	meal_id VARCHAR(7) NOT NULL,
	calorie INT NOT NULL, --must always be specified as it is the main
	FOREIGN KEY(nutrient_id) REFERENCES NUTRITION(nutrition_id),
	FOREIGN KEY(meal_id) REFERENCES MEAL(meal_id)
	);
SELECT * FROM WORKOUT_PLAN

--bulk insert for new tables.
BULK INSERT GYM_REVIEW
FROM 'C:\Users\PC\Downloads\Gym_Review.csv'
WITH(
	FORMAT = 'CSV',
	FIRSTROW = 2
);
SELECT * FROM GYM_REVIEW
DELETE FROM TRAINER_WORKOUTPLAN
DELETE FROM WORKOUT_PLAN
DELETE FROM DAY
DELETE FROM EXERCISE_DAY
DELETE FROM EXERCISE
DELETE FROM MUSCLE
ALTER TABLE WORKOUT_PLAN ALTER COLUMN objective VARCHAR(50);

--workout plan will keel me.

SELECT * FROM GYM_REVIEW

SELECT trainer_id FROM TRAINING_SESSION WHERE member_id = 'M1';

WITH SpecificTrainer AS(
	SELECT trainer_id,gym_id FROM TRAINING_SESSION WHERE member_id = 'M2'
	)
	SELECT T.name,G.name,T.trainer_id,G.gym_id FROM SpecificTrainer ST JOIN TRAINER T ON ST.trainer_id = T.trainer_id JOIN GYM G ON ST.gym_id = G.gym_id; 

SELECT * FROM TRAINER_GYM

WITH specific AS(
	SELECT membership_id FROM MEMBER WHERE member_id = 'M1'
	),
	specificGym AS(
	SELECT gym_id FROM Membership M JOIN specific S on M.membership_id = S.membership_id
	),
	specificTrain AS(
	SELECT trainer_id FROM TRAINER_GYM T JOIN specificGym S ON T.gym_id = S.gym_id
	)
	SELECT T.name,T.trainer_id FROM specificTrain S join TRAINER T on T.trainer_id = S.trainer_id; 
DROP TABLE GYM_REVIEW
DROP TABLE TRAINER_REVIEW
SELECT * FROM GYM_REVIEW
CREATE PROCEDURE AddTrainerReview(
	@trainerID VARCHAR(7),
	@feedback VARCHAR(300),
	@rating FLOAT
	)
AS
BEGIN
	DECLARE @reviewID VARCHAR(7);
	SET @reviewID = (SELECT COUNT(*)+1 FROM TRAINER_REVIEW);
	INSERT INTO TRAINER_REVIEW(review_id,trainer_id,feedback,rating) VALUES (CONCAT('R',@reviewID),@trainerID,@feedback,@rating);
END
DELETE FROM GYM_REVIEW

CREATE PROCEDURE AddGymReview(
	@gymID VARCHAR(7),
	@feedback VARCHAR(300),
	@rating FLOAT
	)
AS
BEGIN
	DECLARE @reviewID VARCHAR(7);
	SET @reviewID = (SELECT COUNT(*)+1 FROM TRAINER_REVIEW);
	INSERT INTO GYM_REVIEW(review_id,gym_id,feedback,rating) VALUES (CONCAT('R',@reviewID),@gymID,@feedback,@rating);
END
EXEC AddGymReview @trainer= ,@feedback= ,@rating= ;
