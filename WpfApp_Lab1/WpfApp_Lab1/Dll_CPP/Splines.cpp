#include "pch.h"
#include <chrono>
#include <iostream>
#include <fstream>
#include "mkl.h"

using namespace std;
// Кубический сплайн для векторно?функци??одни?элементо?
// Вычисляют? значен? функци??двух производны?
// Пр?выполнении информац? выводится ?файл "cpp_errors.txt"

extern "C"  _declspec(dllexport)
void MKLSplines(
    int n, 
    double* x, 
    double* y, 
    double derLeft, 
    double derRight,
    int nN, 
    double* xUni, 
    double* res, 
    int& return_value)
{

    ofstream ofs("cpp_errors.txt", ios_base::out);
    ofs << " n = " << n << "  nN = " << nN << endl;
    ofs << " derLeft = " << derLeft << "  derRight = " << derRight << endl;

    for (int j = 0; j < n; j++)  ofs << "x[" << j << "] = " << x[j] << "  y = " << y[j] << endl;
    ofs << " nN = " << nN << endl;
    for (int j = 0; j < nN; j++)  ofs << "xUni[" << j << "] = " << xUni[j] << endl;
   
    MKL_INT sorder = DF_PP_CUBIC;       // по?до?сплайн?(spline order) 
    MKL_INT stype = DF_PP_NATURAL;      // ти?сплайн?(spline type)

    MKL_INT nx = n;                     // числ?узло?сплайн?(number of break points)
    MKL_INT xhint = DF_NON_UNIFORM_PARTITION;  // значен? сплайн?вычисляют? на неравномерно?сетк?
    //double xUni[2];    // для равномерно?сетк?можн?передать только границ?интервал?интерполирован? 
    //xUniform[0] = xL;
    //xUniform[1] = xR;
  
    MKL_INT ny = 1;                     // размер вектор-функци?(number of functions)
    MKL_INT yhint = DF_NO_HINT;         // дополнительн? информац? не использует?

    MKL_INT nscoeff = ny * (nx - 1) * DF_PP_CUBIC;   // числ?коэффициенто?сплайн?(number of spline coefficients)
    MKL_INT scoeffhint = DF_NO_HINT;                  // дополнительн? информац? не использует?
    double* scoeff = new double[nscoeff];             // массив коэффициенто?сплайн?(array of spline coefficients)

    MKL_INT bc_type = DF_BC_1ST_LEFT_DER | DF_BC_1ST_RIGHT_DER; // ти?граничны?услови?
    double* bc = new double[2]{ derLeft, derRight };   // значен? производно?на концах отрезк?
 
    MKL_INT ic_type = DF_NO_IC;     // ти?услови?во внутренних точках (internal conditions type)
    double* ic;                     // условия во внутренних точках (internal conditions)
    ic = 0;                         // условия во внутренних точках не используют?

    DFTaskPtr task;                     // Создание дескриптор?(Data Fitting task descriptor)
    int errcode = dfdNewTask1D(&task, nx, x, xhint, ny, y, yhint);
    ofs << "dfdNewTask1D errcode = " << errcode << endl;
  
    errcode = dfdEditPPSpline1D(task, sorder, stype, bc_type, bc, ic_type, ic, scoeff, scoeffhint);
    ofs << "dfdEditPPSpline1D errcode = " << errcode << endl;
   
    errcode = dfdConstruct1D(task, DF_PP_SPLINE, DF_METHOD_STD);
    ofs << "dfdConstruct1D errcode = " << errcode << endl;
    MKL_INT nsite = nN;
   
    MKL_INT dorder[3]{ 1 , 1 , 1 };  // вычисляют? значен? функци? первой ?второй производны?
  
    // Вычисление значений сплайн??двух производны??точках site
    int ndorder = 3;
    errcode = dfdInterpolate1D(task, DF_INTERP, DF_METHOD_PP,
                               nsite, xUni, DF_SORTED_DATA,
                               ndorder, dorder, NULL,
                               res, DF_MATRIX_STORAGE_ROWS, NULL);
    ofs << "dfdInterpolate1D errcode = " << errcode << endl;
  
    errcode = dfDeleteTask(&task);
    ofs << "dfDeleteTask errcode = " << errcode << endl;
   
    if (errcode != 0) return_value = -1;
    else return_value = 0;
  
    delete[] scoeff;
    ofs.close();     // закрывае?файл
}