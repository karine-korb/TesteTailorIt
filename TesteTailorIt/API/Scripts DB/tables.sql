--CREATE DATABASE testTailorIt;

use testTailorIt

create table Employee (
	Id int NOT NULL IDENTITY PRIMARY KEY,
	FirstName varchar(20) NOT NULL,
    LastName varchar(200) NOT NULL,
    BirthDate datetime NOT NULL,
    Email varchar(100) NULL,
    Gender char(1) NOT NULL,
	Age int NOT NULL
);


create table Habilities (
	Id int NOT NULL IDENTITY PRIMARY KEY,
	Name varchar(100) NOT NULL
);


create table EmployeeHabilities (
	Id int NOT NULL IDENTITY PRIMARY KEY,
	IdEmployee int NOT NULL FOREIGN KEY REFERENCES Employee(Id),
	IdHability int NOT NULL FOREIGN KEY REFERENCES Habilities(Id)
);


insert into Habilities 
	select 'C#'
insert into Habilities 
	select 'HTML'
insert into Habilities 
	select 'CSS3'
insert into Habilities 
	select 'Angular'
insert into Habilities 
	select 'MVC'
insert into Habilities 
	select 'SQL'
