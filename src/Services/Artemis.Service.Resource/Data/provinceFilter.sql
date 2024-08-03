create table tmp as select
    t.name,
    t.provinceCode,
    t.cityCode,
    t.areaCode,
    t.streetCode,
    t.villageCode,
    t.code,
    t.level,
    t.type
from (
select p.name, p.code, p.code as provinceCode, null as cityCode, null as areaCode, null as streetCode,null as villageCode,  1 as level, 'Province' as type from province p
Union
select c.name, c.code, c.provinceCode, c.code as cityCode, null as areaCode, null as streetCode,null as villageCode, 2 as level, 'Prefecture' as type from city c
UNION
select a.name, a.code, a.provinceCode, a.cityCode, a.code as areaCode, null as streetCode,null as villageCode,  3 as level, 'County' as type from area a
UNION
select s.name, s.code, s.provinceCode, s.cityCode, s.areaCode, s.code as streetCode, null as villageCode, 4 as level, 'Township' as type from street s
UNION
select v.name, v.code, v.provinceCode, v.cityCode, v.areaCode, streetCode, v.code as villageCode, 5 as level, 'Street' as type from village v) t where t.provinceCode = '53';


update tmp a
set pid = b.id
from tmp b
where a.provincecode = b.code
and a.level = 2;

insert into "ArtemisDev"."Resource"."ArtemisDivision"
    (
     "Id",
     "ParentId",
     "Name",
     "FullName",
     "Code",
     "Level",
     "Type",
     "CreatedAt",
     "UpdatedAt",
     "CreateBy",
     "ModifyBy",
     "ConcurrencyStamp"
    )
select 
    id, pid, name, name as fn,code, level, type, now(), now(), '00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000', 'e10ca47b-0782-4232-b7c6-7d2a981942bd' 
from tmp order by level;