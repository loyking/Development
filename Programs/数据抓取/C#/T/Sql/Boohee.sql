create database Boohee
use Boohee

--drop table FoodType
create table FoodType
(
	Id int primary key identity(1,1),
	Content nvarchar(50) not null
)

select * from FoodType

create table Food
(
    Id int primary key identity(1,1),
	FoodType int foreign key references FoodType(Id),
	ImgUrl nvarchar(max) null,
	Title nvarchar(max) null,
	Content nvarchar(max) null
)

select * from Food
select count(*),FoodType from Food group by FoodType order by FoodType