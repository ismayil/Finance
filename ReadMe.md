# Debit Credit Management 
# AngularJS, C# WebApi, Material Design
# Outh2 Token, web authentification

DCM is a app which you can control debit and credit values provided by your departments. Modern technologies used for well UI and back-end

  - web based token authorize
  - JSON two-way data binding
  - AngularJS front-end

# Version
 1.0
# Required 
  - Visual Studio 2015
  - IIS express (usually installed when VS creat)
  - Excel for look exported document

# Installation
  - Launch Finance.sln
  - Run "nuget restore" in nuget package management
  - Change SQL DB connection (name="AuthContext") in Finance->webconfig.config
  - Run "Update-Datbase" in nuget package management
  - Run this command to create new view in SQL (Server Explorer->Databases->AuthContext, right click, New Query): 
```sql
Create View Total as
select departmentName,title,title.Description,Value.DepartmentCode as DepartmentCode, value.debitkredit,date.id as DateId, title+'_'+title.Description as titdes, sum([value]) as [value],dates from Value
left join Department on Value.DepartmentCode = Department.DepartmentCode
left join Title on value.TitleCode = title.id
left join Date on date.ID = Value.DateId
GROUP BY departmentName,title,title.Description,Value.DepartmentCode,  value.debitkredit,date.id,  title+'-'+title.Description,dates;
```
- Run this command to create new Procedure in SQL (Server Explorer->Databases->AuthContext, right click, New Query): 
```sql
CREATE PROCEDURE [dbo].[pr_total]
@DateId int,
@DepartmentCode int,
@debitkredit int

AS
--departmentName,title,description,departmentCode, debitkredit,dateid,titdes,values,dates
DECLARE @PivotQuery AS NVARCHAR(MAX)
DECLARE @PivotTotal AS NVARCHAR(MAX)
DECLARE @ColumnName AS NVARCHAR(MAX)
DECLARE @ColumnNameSelect AS NVARCHAR(MAX)
DECLARE @DateWhere as NVARCHAR(MAX)
DECLARE @DepartmentWhere as NVARCHAR(MAX)
SELECT @ColumnName = ISNULL(@ColumnName + ',','') 
       + lower(QUOTENAME(title+'_'+description))
FROM (SELECT DISTINCT title,Description FROM title where typeint=@debitkredit) AS titles
SELECT @ColumnNameSelect = ISNULL(@ColumnNameSelect + ',','') 
       + 'SUM ('+lower(QUOTENAME(title+'_'+description))+') as '+ lower(QUOTENAME(title+'_'+description))
FROM (SELECT DISTINCT title,Description FROM title where typeint=@debitkredit) AS titles
Select @DateWhere = ' 1=1 '
Select @DepartmentWhere = ' 1=1 '
if (@DateId !=0)
	begin 
	Select @DateWhere = ' DateID='+convert(varchar(3),@DateId)
end
if(@DepartmentCode !=0)
	begin 
	Select @DepartmentWhere = ' DepartmentCode='+convert(varchar(4),@DepartmentCode)
end
SET @PivotQuery = 
  N'SELECT departmentName,dates,'+@ColumnNameSelect+'
    FROM (select * from total WHERE DebitKredit='+convert(varchar(3),@debitkredit)+' and  '+@DepartmentWhere+' and '+@DateWhere+') P
			PIVOT(sum(value)
					FOR titdes IN (' + @ColumnName + ')) AS PVTTable
				group by departmentName,dates
 '

SET @PivotTotal = 
  N'SELECT * from Total WHERE DebitKredit='+convert(varchar(3),@debitkredit)+' and  '+@DepartmentWhere+' and '+@DateWhere+' '
Exec sp_executesql @PivotQuery
```
- Change all ServiceBase variable in app/service/*.js to your api running url.
# URL
```url
admin: http://yoururl.com/#admin
user: http://yoururl.com/#
```
License
----

MIT