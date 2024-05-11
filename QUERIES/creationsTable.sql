-- Table Creation.

--OWNER
CREATE TABLE [OWNER]
(
	owner_id VARCHAR(7) PRIMARY KEY NOT NULL,
	[name] VARCHAR(30) NOT NULL,
	username VARCHAR(100) NOT NULL,
	[password] VARCHAR(100) NOT NULL,
	email VARCHAR(100) NOT NULL
);

--GYM
CREATE TABLE GYM
(
	gym_id VARCHAR(7) PRIMARY KEY NOT NULL,
	owner_id VARCHAR(7) NOT NULL,
	[name] VARCHAR(50) NOT NULL,
	membership_growth_rate FLOAT,
	financial_performance VARCHAR(100),
	no_of_members INT,
    	rating FLOAT,
	[location] VARCHAR(50) NOT NULL,
	[password] VARCHAR(100) NOT NULL,
	email VARCHAR(100) NOT NULL,

	FOREIGN KEY(owner_id) REFERENCES [OWNER](owner_id)
);

--ADMIN
CREATE TABLE [ADMIN]
(
	admin_id VARCHAR(7) PRIMARY KEY NOT NULL,
	[name] VARCHAR(30) NOT NULL,
	[password] VARCHAR(100) NOT NULL,
	email VARCHAR(100) NOT NULL
);

--TRAINER
CREATE TABLE TRAINER
(
	trainer_id VARCHAR(7) PRIMARY KEY NOT NULL,
	[name] VARCHAR(30) NOT NULL,
	username VARCHAR(100) NOT NULL,
	[password] VARCHAR(100) NOT NULL,
	email VARCHAR(100) NOT NULL,
	experience VARCHAR(20),
	clients INT,
	rating FLOAT,
);

-- Trainer_Gym
CREATE TABLE TRAINER_GYM
(
	-- Composite Primary Key
	trainer_id VARCHAR(7) NOT NULL,
	gym_id VARCHAR(7) NOT NULL,
	
    PRIMARY KEY (trainer_id, gym_id),
	
	FOREIGN KEY(trainer_id) REFERENCES TRAINER(trainer_id),
	FOREIGN KEY(gym_id) REFERENCES GYM(gym_id)
);

-- Speciality
CREATE TABLE SPECIALITY
(
	-- Composite Primary Key
	speciality_id VARCHAR(7) NOT NULL,
	trainer_id VARCHAR(7) NOT NULL,
	[name] VARCHAR(100) NOT NULL,

    PRIMARY KEY (trainer_id, speciality_id),
	FOREIGN KEY(trainer_id) REFERENCES TRAINER(trainer_id)
);

-- Trainer_Workout-Plan
CREATE TABLE TRAINER_WORKOUTPLAN
(
	-- Composite Primary Key
	trainer_id VARCHAR(7) NOT NULL,
	workoutPlan_id VARCHAR(7) NOT NULL,
	
    PRIMARY KEY (trainer_id, workoutPlan_id),
	
	FOREIGN KEY(trainer_id) REFERENCES TRAINER(trainer_id),
	FOREIGN KEY(workoutPlan_id) REFERENCES WORKOUT_PLAN(workoutPlan_id)
);

-- Trainer_Diet-Plan
CREATE TABLE TRAINER_DIETPLAN
(
	-- Composite Primary Key
	trainer_id VARCHAR(7) NOT NULL,
	dietPlan_id VARCHAR(7) NOT NULL,

    PRIMARY KEY (trainer_id, dietPlan_id),
	
	FOREIGN KEY(trainer_id) REFERENCES TRAINER(trainer_id),
	FOREIGN KEY(dietPlan_id) REFERENCES DIET_PLAN(dietPlan_id)
);


--MEMBER
CREATE TABLE [MEMBER]
(
	member_id VARCHAR(7) PRIMARY KEY NOT NULL,
	membership_id VARCHAR(7) NOT NULL,
	workoutPlan_id VARCHAR(7) NOT NULL,
	dietPlan_id VARCHAR(7) NOT NULL,
	[name] VARCHAR(30) NOT NULL,
	[weight] INT,
	email VARCHAR(100) NOT NULL,
	[password] VARCHAR(100) NOT NULL,
	verified INT, --for the admin to verify a user and a user can only log in once they are verified.
	FOREIGN KEY(membership_id) REFERENCES MEMBERSHIP(membership_id),
	FOREIGN KEY(workoutPlan_id) REFERENCES WORKOUT_PLAN(workoutPlan_id),
	FOREIGN KEY(dietPlan_id) REFERENCES DIET_PLAN(dietPlan_id)
);

DROP TABLE MEMBER
CREATE TABLE MEMBER_DIET(
	member_id VARCHAR(7) NOT NULL,
	diet_id VARCHAR(7) NOT NULL,
	--and some other specifier maybe
	FOREIGN KEY(member_id) REFERENCES MEMBER(member_id),
	FOREIGN KEY(diet_id) REFERENCES DIET_PLAN(dietPlan_id));
-- Membership
CREATE TABLE Membership 
(
  membership_id VARCHAR(7) PRIMARY KEY NOT NULL,
  gym_id VARCHAR(7) NOT NULL,
  expiry DATE NOT NULL,
  [type] VARCHAR(50) NOT NULL,

  FOREIGN KEY (gym_id) REFERENCES GYM(gym_id)
);

--WORKOUT PLAN
CREATE TABLE WORKOUT_PLAN
(
	workoutPlan_id VARCHAR(7) PRIMARY KEY NOT NULL,
	objective VARCHAR(100) NOT NULL,
	guidelines VARCHAR(7) NOT NULL,
	difficulty_level VARCHAR(7) NOT NULL,
);

-- DAY
CREATE TABLE [DAY]
(
	day_id VARCHAR(7) NOT NULL,
	workoutPlan_id VARCHAR(7) NOT NULL,
	[dayOfWeek] VARCHAR(10) NOT NULL,
	
    PRIMARY KEY (day_id, workoutPlan_id),
	FOREIGN KEY (workoutPlan_id) REFERENCES WORKOUT_PLAN(workoutPlan_id)
);

-- Exercise_Day
CREATE TABLE EXERCISE_DAY
(
	exercise_id VARCHAR(7) NOT NULL,
	day_id VARCHAR(7) NOT NULL,
	workoutPlan_id VARCHAR(7) NOT NULL,
	
    PRIMARY KEY (day_id, exercise_id),

	FOREIGN KEY(exercise_id) REFERENCES EXERCISE(exercise_id),
	FOREIGN KEY(day_id, workoutPlan_id) REFERENCES [DAY](day_id, workoutPlan_id)
);

-- EXERCISE
CREATE TABLE EXERCISE
(
	exercise_id VARCHAR(7) PRIMARY KEY NOT NULL,
	day_id VARCHAR(7) NOT NULL,
	workoutPlan_id VARCHAR(7) NOT NULL,
	equipment_name VARCHAR(100) NOT NULL,
	[name] VARCHAR(30) NOT NULL,
	[sets] INT NOT NULL,
	reps INT NOT NULL,
	restIntervals INT NOT NULL,
	
	FOREIGN KEY(day_id, workoutPlan_id) REFERENCES [DAY](day_id, workoutPlan_id)
);

-- Equipment
CREATE TABLE EQUIPMENT
(
	-- Composite Primary Key
	equipment_id VARCHAR(7) PRIMARY KEY NOT NULL,
	gym_id VARCHAR(7),
	[name] VARCHAR(30) NOT NULL
	
	FOREIGN KEY(gym_id) REFERENCES GYM(gym_id)
);

-- Muscle
CREATE TABLE MUSCLE
(
	-- Composite Primary Key
	muscle_id VARCHAR(7) NOT NULL,
	exercise_id VARCHAR(7),
	[name] VARCHAR(30) NOT NULL,

    PRIMARY KEY (exercise_id, muscle_id),
	FOREIGN KEY(exercise_id) REFERENCES EXERCISE(exercise_id)
);

-- Diet Plan
CREATE TABLE DIET_PLAN
(
	dietPlan_id VARCHAR(7) PRIMARY KEY NOT NULL,
	meal_id VARCHAR(7) UNIQUE NOT NULL, --each diet plan consists of a meal so we link directly to a meal from the diet plan
	[type] VARCHAR(100) NOT NULL, --breakfast lunch dinner etc.
	objective VARCHAR(100) NOT NULL,
	guidelines VARCHAR(200) NOT NULL,
	difficulty_level VARCHAR(100) NOT NULL
);

-- Meal
CREATE TABLE MEAL
(
	meal_id VARCHAR(7) NOT NULL,
	--removed the diet plan key in the meal table
	--now the meal takes in a nutrition id where it specifies which nutrition it needs to have or contains.
	nutrition_id VARCHAR(7) NOT NULL,
	
    PRIMARY KEY (meal_id),
	FOREIGN KEY(nutrition_id) REFERENCES NUTRITION(nutrition_id)
);

-- Nutrition
CREATE TABLE NUTRITION
(
    nutrition_id VARCHAR(7) NOT NULL,
    --does not take a meal id instead it now has a linked allergen id.
	allergen_id VARCHAR(7) NOT NULL,
    [name] VARCHAR(30) UNIQUE NOT NULL, --added a unique constraint to the name of the nutrition as it is a candidate key and user can search for a meal containing this nutrition.
    [unit] VARCHAR(30) NOT NULL,
    [quantity] VARCHAR(30) NOT NULL,
calories INT NOT NULL, --for now keeping calories as int value not decica.
    PRIMARY KEY (nutrition_id),
    FOREIGN KEY(allergen_id) REFERENCES ALLERGEN(allergen_id)
);

-- Allergen
CREATE TABLE ALLERGEN
(
	allergen_id VARCHAR(7) NOT NULL,
	[name] VARCHAR(30) UNIQUE NOT NULL,
	PRIMARY KEY (allergen_id)
    --FOREIGN KEY(meal_id, dietPlan_id) REFERENCES MEAL(meal_id, dietPlan_id) removing for now.
);

--meal_nutrient
CREATE TABLE meal_nutrient(
	--consists of meal and nutrient id as a meal can have many nutrient and many nutrients can be found in many meals.
	nutrient_id VARCHAR(7) NOT NULL,
	meal_id VARCHAR(7) NOT NULL,
	calorie INT NOT NULL, --must always be specified as it is the main
	FOREIGN KEY(nutrient_id) REFERENCES NUTRITION(nutrition_id),
	FOREIGN KEY(meal_id) REFERENCES MEAL(meal_id)
	);

-- Trainer Review
CREATE TABLE TRAINER_REVIEW
(
	review_id VARCHAR(7) NOT NULL,
	trainer_id VARCHAR(7) NOT NULL,
	feedback VARCHAR(300),
	rating FLOAT NOT NULL,
	PRIMARY KEY (review_id),
	FOREIGN KEY(trainer_id) REFERENCES TRAINER(trainer_id)
);

-- Gym Review
CREATE TABLE GYM_REVIEW
(
	review_id VARCHAR(7) NOT NULL,
	gym_id VARCHAR(7) NOT NULL,
	feedback VARCHAR(300),
	rating FLOAT NOT NULL,
	PRIMARY KEY (review_id),
	FOREIGN KEY(gym_id) REFERENCES GYM(gym_id)
);

-- Verification Request
CREATE TABLE VERIFICATION_REQUEST
(
	request_id VARCHAR(7) NOT NULL,
	trainer_id VARCHAR(7) NOT NULL,
	
	PRIMARY KEY (trainer_id, request_id),
	FOREIGN KEY(trainer_id) REFERENCES TRAINER(trainer_id)
);

-- Registration Request
CREATE TABLE REGISTRATION_REQUEST
(
	request_id VARCHAR(7) NOT NULL,
	gym_id VARCHAR(7) NOT NULL,
	
	PRIMARY KEY (gym_id, request_id),
	FOREIGN KEY(gym_id) REFERENCES GYM(gym_id)
);

-- Training Session
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
