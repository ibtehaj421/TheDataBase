-- Queries

-- Query 2
SELECT  *
FROM MEMBER
JOIN (
	SELECT GYM.name AS 'Gym_Name', MEMBERSHIP.membership_id
	FROM GYM
	JOIN Membership
	ON GYM.gym_id = Membership.gym_id
	WHERE GYM.name = 'SYN'
) AS RES1
ON MEMBER.membership_id = RES1.membership_id
WHERE Member.dietPlan_id = 1

-- Query 4
SELECT COUNT(RES4.membership_id) AS [Total Machine Users]
FROM EXERCISE
JOIN (
	SELECT EXERCISE_DAY.exercise_id, EXERCISE_DAY.day_id, RES3.membership_id
	FROM EXERCISE_DAY
	JOIN (
		SELECT day_id, RES2.membership_id
		FROM DAY
		JOIN (
			SELECT workoutPlan_id, RES1.membership_id
			FROM MEMBER
			JOIN (
				SELECT GYM.name AS 'Gym_Name', MEMBERSHIP.membership_id
				FROM GYM
				JOIN Membership
				ON GYM.gym_id = Membership.gym_id
				WHERE GYM.name = 'SYN'
			) AS RES1
			ON MEMBER.membership_id = RES1.membership_id
		) AS RES2
		ON DAY.workoutPlan_id = RES2.workoutPlan_id
		WHERE DAY.dayOfWeek = 'Saturday'
	) AS RES3
	ON EXERCISE_DAY.day_id = RES3.day_id
) AS RES4
ON EXERCISE.exercise_id = RES4.exercise_id
WHERE eqiupment_name LIKE '%Machine%'

-- Query 10
SELECT RES1.name AS [Gym Name], COUNT(membership_id) AS [Number of Members]
FROM (
	SELECT Membership.membership_id, Gym.name, expiry
	FROM Membership
	JOIN GYM
	ON Membership.gym_id = GYM.gym_id
) AS RES1
WHERE expiry >= DATEADD(MONTH, -6, GETDATE())
GROUP BY RES1.name



--Query number 3 for the diet plans with less than 500 calories.
--we will be using only one nutrition per diet plan


WITH LowCal AS(
	SELECT meal_id AS mID,SUM(calorie) AS cals FROM meal_nutrient GROUP BY meal_id
	)
SELECT * FROM DIET_PLAN DP JOIN (SELECT mID FROM LowCal WHERE cals < 500) AS RES ON DP.meal_id = RES.mID
;


--report 1 quer
--we will run a few different queries.
--first list all the gym trainers of a specific gym
--now for the admin this feature feeds a gym id 
--but for the gym owner the query is a but modified as the gym owner does not have access to other gym ids. we will search on basis of the owner id and then extract a gym id.

SELECT Trainer_id FROM TRAINER_GYM WHERE gym_id = 1; --some specified value.
--now the second select will be shown on the screen as which trainer would you like to view.
--the main query begins here.
WITH MemberList AS(
	SELECT member_id FROM TRAINING_SESSION WHERE trainer_id = 1) --some specified value.
SELECT M.member_id,M.name FROM MEMBER M JOIN MemberList ML ON M.member_id = ML.member_id;


--report 3
--all members using a trainers diet plan

SELECT dietPlan_id FROM TRAINER_DIETPLAN WHERE trainer_id = 1 --somespecific value.
--this shows all the diet plans 
--then the user will select a diet plan id and it will show which members are using this diet plan
WITH RES AS(
SELECT member_id FROM MEMBER_DIET WHERE diet_id = 1) -- some specified value.
SELECT M.member_id,M.name FROM MEMBER M JOIN RES R ON M.member_id = R.member_id;
