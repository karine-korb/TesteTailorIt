--Procedures


create procedure GetAllHabilities
as
select * from Habilities
go


create procedure GetAllEmployees
as
begin
	select 
		a.Id, 
		a.FirstName, 
		a.LastName, 
		a.Email, 
		a.BirthDate,
		a.Age,
		a.Gender,
		c.Id as IdHability,
		c.Name as NameHability
	from Employee a
	inner join EmployeeHabilities b
		on a.Id = b.IdEmployee
	inner join Habilities c
		on b.IdHability = c.Id
end
go


create procedure GetEmployeesByFilter
@Age int,
@Gender char(1)
as
begin

declare @where int;
set @where= 0;

declare @sql nvarchar(max);
set @sql = '';

set @sql = @sql + 'select ';
set @sql = @sql + '		a.Id, ';
set @sql = @sql + '		a.FirstName, ';
set @sql = @sql + '		a.LastName, ';
set @sql = @sql + '		a.Email, ';
set @sql = @sql + '		a.BirthDate, ';
set @sql = @sql + '		a.Age, ';
set @sql = @sql + '		a.Gender, ';
set @sql = @sql + '		c.Id as IdHability, ';
set @sql = @sql + '		c.Name as NameHability ';
set @sql = @sql + 'from Employee a ';
set @sql = @sql + 'inner join EmployeeHabilities b ';
set @sql = @sql + '		on a.Id = b.IdEmployee ';
set @sql = @sql + 'inner join Habilities c ';
set @sql = @sql + '		on b.IdHability = c.Id ';

if(@Gender != '' )
begin
	if(@where = 0)
	begin
		set @sql = @sql + 'where a.Gender = ''' + @Gender + ''' ';
		set @where = 1;
	end
	else
	begin
		set @sql = @sql + 'and a.Gender = ''' + @Gender + ''' ';
	end
end

if(@Age != '')
begin
	if(@where = 0)
	begin
		set @sql = @sql + 'where a.Age = ' + CAST(@Age as varchar(10)) + ' ';
		set @where = 1;
	end
	else
	begin
		set @sql = @sql + 'and a.Age = ' + CAST(@Age as varchar(10)) + ' ';
	end
end

exec sp_executesql @sql

end
go


create procedure GetEmployeesById
@Id int
as
begin
	select 
		a.Id, 
		a.FirstName, 
		a.LastName, 
		a.Email, 
		a.BirthDate,
		a.Age,
		a.Gender,
		c.Id as IdHability,
		c.Name as NameHability
	from Employee a
	inner join EmployeeHabilities b
		on a.Id = b.IdEmployee
	inner join Habilities c
		on b.IdHability = c.Id
	where a.Id = @Id
end
go


create procedure InsertEmployee
@FirstName varchar(20),
@LastName varchar(200),
@BirthDate datetime,
@Age int,
@Email varchar(100),
@Gender char(1)
as
begin
insert into Employee (FirstName, LastName, BirthDate, Age, Email, Gender)
	values (@FirstName, @LastName, @BirthDate, @Age, @Email, @Gender)

select Scope_Identity() 
end
go


create procedure InsertEmployeeHabilities
@IdEmployee int,
@IdHability int
as
begin
insert into EmployeeHabilities (IdEmployee, IdHability)
	values (@IdEmployee, @IdHability)
end
go


create procedure UpdateEmployee
@Id int,
@FirstName varchar(20),
@LastName varchar(200),
@BirthDate datetime,
@Age int,
@Email varchar(100),
@Gender char(1),
@IdHability int
as
begin
	update Employee 
		set FirstName = @FirstName, 
		LastName = @LastName, 
		BirthDate = @BirthDate, 
		Age = @Age,
		Email = @Email, 
		Gender = @Gender
	where Id = @Id 

	update EmployeeHabilities 
		set IdHability = @IdHability
	where IdEmployee = @Id 
end
go


create procedure DeleteEmployee
@Id int
as
begin
	delete EmployeeHabilities
	where IdEmployee = @Id 

	delete Employee
	where Id = @Id 
end
go



