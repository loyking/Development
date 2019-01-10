create database ExamQuery
use ExamQuery

--考生信息表
create table StudentInfo
(
	Id int primary key identity(1,1),
	StudentName varchar(10)  null,
	StudentId varchar(20) null,
	Sfz char(18)  null,
	Zkzh varchar(15)  null,
	ProvinceKey int  null,
	CityKey int null
)

select * from StudentInfo
--高考分数表
create table GkScore(
	Id int primary key identity(1,1),
	StudentId int foreign key references StudentInfo(Id),
	SubjectType char(4)  null,
	ChineseScore float  null,
	MathematicsScore float  null,
	EnglishScore float  null,
	ComprehensiveScore float  null,
	SumScore float  null,
)


--高考录取结果表
create table GkAdmissions
(
	Id int primary key identity(1,1),
	StudentId int foreign key references StudentInfo(Id),
	Batch nvarchar(20)  null,
	Category nvarchar(20)  null,
	SchoolName nvarchar(20)  null
)

--中考分数表
create table ZkScore(
	Id int primary key identity(1,1),
	StudentId int foreign key references StudentInfo(Id),
	ChineseScore float  null,
	MathematicsScore float  null,
	EnglishScore float  null,
	HistoryScore float  null,
	GeographyScore float  null,
	BiologyScore float  null,
	ChemistryScore float  null,
	PhysicsScore float  null,
	PoliticsScore float  null,
	Sports float null,
	SumScore float  null
)

--中考录取表
create table ZkAdmissions(
	Id int primary key identity(1,1),
	StudentId int foreign key references StudentInfo(Id),
	SchoolName nvarchar(20) null,
	Batch nvarchar(20)  null,
	Category nvarchar(20)  null,
)

--临时表
create table Temporary(
	Id int primary key identity(1,1),
	ProvinceKey nvarchar(10),
	DataJson nvarchar(max) not null
)
