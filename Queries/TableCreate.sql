Use [VirtualHR]

IF OBJECT_ID('tblExerciseResults') IS NOT NULL
BEGIN
 DROP TABLE tblExerciseResults
END
CREATE TABLE tblExerciseResults
(
 [repNumber] INT,
 [repTime] FLOAT,
 [exerciseID] INT
)

IF OBJECT_ID('tblExercises') IS NOT NULL
BEGIN
 DROP TABLE tblExercises
END
CREATE TABLE tblExercises
(
 [exerciseID] INT IDENTITY(1,1) PRIMARY KEY ,
 [userID] INT,
 [exercise] NVARCHAR(25),
 [bestTimeOccured] BIT,
 [bestTime] FLOAT,
 [timeStamp] DATETIME
)

IF OBJECT_ID('tblUserDetails') IS NOT NULL
Begin
 DROP TABLE tblUserDetails
END
CREATE TABLE tblUserDetails
(
 [userID] INT IDENTITY(1,1) PRIMARY KEY,
 [accountType] BIT, -- 0 Patient, 1 Clinican
 [fullName] NVARCHAR(50),
 [address] NVARCHAR(50),
 [email] NVARCHAR(50),
 [postCode] NVARCHAR(20)
)

ALTER TABLE tblExerciseResults
ADD CONSTRAINT ExerciseID_FK
FOREIGN KEY (ExerciseId)
REFERENCES tblExercises(ExerciseId)
ON DELETE CASCADE;

ALTER TABLE tblExercises
ADD CONSTRAINT userID_FK
FOREIGN KEY (userID)
REFERENCES tblUserDetails(userID)
ON DELETE CASCADE;