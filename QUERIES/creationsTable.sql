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
	[name] VARCHAR(30) NOT NULL,

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
	[name] VARCHAR(30) NOT NULL,
	[weight] INT,

	FOREIGN KEY(membership_id) REFERENCES MEMBERSHIP(membership_id),
	FOREIGN KEY(workoutPlan_id) REFERENCES WORKOUT_PLAN(workoutPlan_id)
);

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
	objective VARCHAR(30) NOT NULL,
	guidelines VARCHAR(7) NOT NULL,
	difficulty_level VARCHAR(7) NOT NULL,
);

-- DAY
CREATE TABLE [DAY]
(
	day_id VARCHAR(7) PRIMARY KEY NOT NULL,
	workoutPlan_id VARCHAR(7) NOT NULL,
	[dayOfWeek] VARCHAR(10) NOT NULL,
	
	FOREIGN KEY (workoutPlan_id) REFERENCES WORKOUT_PLAN(workoutPlan_id)
);

-- Exercise_Day
CREATE TABLE EXERCISE_DAY
(
	exercise_id VARCHAR(7) NOT NULL,
	day_id VARCHAR(7) NOT NULL,
	
    PRIMARY KEY (day_id, exercise_id),

	FOREIGN KEY(exercise_id) REFERENCES EXERCISE(exercise_id),
	FOREIGN KEY(day_id) REFERENCES DAY(day_id)
);

-- EXERCISE
CREATE TABLE EXERCISE
(
	exercise_id VARCHAR(7) PRIMARY KEY NOT NULL,
	day_id VARCHAR(7) NOT NULL,
	[name] VARCHAR(30) NOT NULL,
	eqiupment_name VARCHAR(30) NOT NULL,
	[sets] INT NOT NULL,
	reps INT NOT NULL,
	restIntervals INT NOT NULL,
	
	FOREIGN KEY(day_id) REFERENCES [DAY](day_id)
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
	[type] VARCHAR(30) NOT NULL,
	objective VARCHAR(30) NOT NULL,
	guidelines VARCHAR(200) NOT NULL,
	difficulty_level VARCHAR(7) NOT NULL
);

-- Meal
CREATE TABLE MEAL
(
	meal_id VARCHAR(7) NOT NULL,
	dietPlan_id VARCHAR(7) NOT NULL,
	
    PRIMARY KEY (meal_id, dietPlan_id),
	FOREIGN KEY(dietPlan_id) REFERENCES DIET_PLAN(dietPlan_id)
);

-- Nutrition
CREATE TABLE NUTRITION
(
    nutrition_id VARCHAR(7) NOT NULL,
    meal_id VARCHAR(7) NOT NULL,
	dietPlan_id VARCHAR(7) NOT NULL,
    [name] VARCHAR(30) NOT NULL,
    [unit] VARCHAR(30) NOT NULL,
    [quantity] VARCHAR(30) NOT NULL,

    PRIMARY KEY (nutrition_id, meal_id),
    FOREIGN KEY(meal_id, dietPlan_id) REFERENCES MEAL(meal_id, dietPlan_id)
);

-- Allergen
CREATE TABLE ALLERGEN
(
	allergen_id VARCHAR(7) NOT NULL,
	meal_id VARCHAR(7) NOT NULL,
	dietPlan_id VARCHAR(7) NOT NULL,
	[name] VARCHAR(30) NOT NULL,
	
	PRIMARY KEY (allergen_id, meal_id),
    FOREIGN KEY(meal_id, dietPlan_id) REFERENCES MEAL(meal_id, dietPlan_id)
);

-- Trainer Review
CREATE TABLE TRAINER_REVIEW
(
	review_id VARCHAR(7) NOT NULL,
	trainer_id VARCHAR(7) NOT NULL,
	feedback VARCHAR(300),
	rating FLOAT NOT NULL,

	PRIMARY KEY (trainer_id, review_id),
	FOREIGN KEY(trainer_id) REFERENCES TRAINER(trainer_id)
);

-- Gym Review
CREATE TABLE GYM_REVIEW
(
	review_id VARCHAR(7) NOT NULL,
	gym_id VARCHAR(7) NOT NULL,
	feedback VARCHAR(300),
	rating FLOAT NOT NULL,
	
	PRIMARY KEY (gym_id, review_id),
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
