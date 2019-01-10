create database ExamQuery
use ExamQuery

--考生信息表
create table StudentInfo
(
	Id int primary key identity(1,1),
	StudentName varchar(10) not null,
	Sfz char(18) not null,
	Zkzh varchar(15) not null,
	ProvinceKey int not null,
	CityKey int null
)

select * from StudentInfo
select Id from StudentInfo where Sfz= 430422111111111111 and Zkzh=03123465
--delete from StudentInfo where Id>1
--drop table GkAdmissions
select * from GkAdmissions

--高考录取结果表
create table GkAdmissions
(
	Id int primary key identity(1,1),
	StudentId int foreign key references StudentInfo(Id),
	Batch nvarchar(20) not null,
	Category nvarchar(20) not null,
	SchoolName nvarchar(20) not null
)

--高考分数表
create table GkScore(
	Id int primary key identity(1,1),
	StudentId int foreign key references StudentInfo(Id),
	SubjectType char(4) not null,
	ChineseScore float not null,
	MathematicsScore float not null,
	EnglishScore float not null,
	ComprehensiveScore float not null,
	SumScore float not null,
)
select*from GkScore
select count(1) from GkScore where StudentId = 100

select * from ZkAdmissions
--中考录取表
create table ZkAdmissions(
	Id int primary key identity(1,1),
	StudentId int foreign key references StudentInfo(Id),
	SchoolName nvarchar(20) not null
)

select * from ZkScore
--中考分数表
create table ZkScore(
	Id int primary key identity(1,1),
	StudentId int foreign key references StudentInfo(Id),
	ChineseScore float not null,
	MathematicsScore float not null,
	EnglishScore float not null,
	HistoryScore float not null,
	GeographyScore float not null,
	BiologyScore float not null,
	ChemistryScore float not null,
	PhysicsScore float not null,
	PoliticsScore float not null,
	SumScore float not null
)