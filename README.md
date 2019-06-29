# DemoDataTable
测试DataTable的查询效率

Csv2DataTable(): 213.4315 ms
dt.Select(): 614.3553 ms
dt.Rows.Find(): 7.9794 ms

结论：联合多个字段充当一个主键来查询的效率真高!
