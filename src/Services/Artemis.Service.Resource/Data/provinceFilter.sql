select
    name,
    code,
    level,
    type
from (
select name, code, code as provinceCode, 1 as level, 'Province' as type from province
Union
select name, code, provinceCode, 2 as level, 'Prefecture' as type from city
UNION
select name, code, provinceCode, 3 as level, 'County' as type from area
UNION
select name, code, provinceCode, 4 as level, 'Township' as type from street
UNION
select name, code, provinceCode, 5 as level, 'Street' as type from village) t where provinceCode = 53;