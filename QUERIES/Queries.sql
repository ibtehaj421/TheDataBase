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
SELECT COUNT(member_id) AS [No. of Members]
FROM DAY
JOIN (
	SELECT member_id, workoutPlan_id, [Equipment Name]
	FROM MEMBER
	JOIN (
		SELECT membership_id, RES1.name AS [Equipment Name]
		FROM MEMBERSHIP
		JOIN (
			SELECT *
			FROM EQUIPMENT
			WHERE gym_id = 1
		) AS RES1
		ON Membership.gym_id = RES1.gym_id
	) AS RES2
	ON MEMBER.membership_id = RES2.membership_id
) AS RES3
ON DAY.workoutPlan_id = RES3.workoutPlan_id
WHERE dayOfWeek = 'Saturday' AND [Equipment Name] = 'Cable Machine'
	
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


--report 1 querY
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


--------------------------
--- Additional Queries ---
--------------------------

-- 1. Admin : Compare Ratings of all Gyms.
SELECT name AS [Gym Name], rating
FROM GYM
ORDER BY rating DESC

-- 2. Gym : Find all members whose membership has expired.
SELECT MEMBER.name, MEMBER.weight, MEMBER.member_id, MEMBER.membership_id, expiry
FROM Membership
JOIN MEMBER
ON Membership.membership_id = MEMBER.membership_id
WHERE gym_id = 1 AND expiry < GETDATE()

-- 3. Gym : Compare the usage of eqiupment
SELECT equipment_name, COUNT(equipment_name) AS [People Using]
FROM MEMBERSHIP
JOIN (
	SELECT equipment_name, MEMBER.membership_id
	FROM EXERCISE
	JOIN MEMBER
	ON EXERCISE.workoutPlan_id = MEMBER.workoutPlan_id
) AS RES1
ON MEMBERSHIP.membership_id = RES1.membership_id
WHERE gym_id = 1
GROUP BY equipment_name

-- 4. Admin/Trainer/Member : Find all gyms that have specific equipment
SELECT DISTINCT NAME 
FROM GYM
JOIN (
	SELECT gym_id
	FROM MEMBERSHIP
	JOIN (
		SELECT equipment_name, MEMBER.membership_id
		FROM EXERCISE
		JOIN MEMBER
		ON EXERCISE.workoutPlan_id = MEMBER.workoutPlan_id
		WHERE equipment_name = 'Dumbbells'
	) AS RES1
	ON MEMBERSHIP.membership_id = RES1.membership_id
) AS RES2
ON GYM.gym_id = RES2.gym_id
