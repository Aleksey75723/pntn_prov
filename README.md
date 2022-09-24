# pntn_prov
To check the PNTN blocks.
Программа предназначена для расчёта погрешностей после ввода измеренных данных, измеренные данные берутся с вольтметра и амперметра при подключении блока ПНТН по соответствующим схемам проверок(вкладка "Схемы проверок"). Программа рассчитывает и показывает входит ли результат в допустимую погрешность(диапазон допустимой погрешности может меняться вручную в окне программы). Вычисления производятся по формулам с использованием констант, перечень констант находится в файле "pntn_const.txt"(находится в директории программы), в случае если конструктивно блок изменится и изменятся константы, их легко можно поменять в вышеуказанном txt файле. Так же есть функция сохранения проверки в протокол "pntn_protokol.txt" с указанием даты проверки, номера блока, даты изготовления и всех вычислений. 
