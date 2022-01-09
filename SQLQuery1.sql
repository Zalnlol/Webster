create database WebsterDB
go

use WebsterDB
go

create table tb_User
(
	Email varchar(100) primary key,
	[Password] varchar(100),
	Phone varchar(20),
	Birthday datetime,
	Avatar varchar(100),
)

create table tb_Exam
(
	ExamId int primary key identity(1, 1),
	ExamName varchar(100),
	ExamType bit,
	PassPercent int,
	FirstCountdown time,
	SecondCountdown time,
	ThirdCountdown time,
	StartDate datetime,
	FinishTime datetime,
)
go

create table tb_Candidate_List
(
	CandidateId int primary key identity(1, 1),
	Email varchar(100) foreign key references tb_User(Email),
	ExamId int foreign key references tb_Exam(ExamId)
)
go

create table tb_Result
(
	ResultId int primary key identity(1, 1),
	Email varchar(100) foreign key references tb_User(Email),
	ExamId int foreign key references tb_Exam(ExamId),
	GKScore int,
	MathScore int,
	TechScore int,
	ExamDate datetime,
	CorrectAnsGK int,
	CorrectAnsMath int,
	CorrectAnsTech int,
	TotalQuestionGK int,
	TotalQuestionMath int,
	TotalQuestionTech int,
	IsPass bit,
)
go

create table tb_Question
(
	QuestionId int primary key identity(1, 1),
	[Subject] varchar(100),
	QuestionTitle varchar(100),
	QuestionType bit,
	Photo varchar(100),
)

create table tb_ExamType
(
	ExamTypeId int identity(1, 1),
	ExamId int foreign key references tb_Exam(ExamId),
	QuestionId int foreign key references tb_Question(QuestionId),
	Mark int,
)

create table tb_Answer
(
	AnswerId int primary key identity(1, 1),
	QuestionId int foreign key references tb_Question(QuestionId),
	AnswerContent varchar(100),
	Photo varchar(100),
	IsCorrectAnswer bit,
)

select * from tb_User
select * from tb_Exam
select * from tb_Candidate_List
select * from tb_Result
select * from tb_Question
select * from tb_Answer
