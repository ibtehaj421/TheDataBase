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