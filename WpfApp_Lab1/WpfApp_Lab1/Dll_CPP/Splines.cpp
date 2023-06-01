#include "pch.h"
#include <chrono>
#include <iostream>
#include <fstream>
#include "mkl.h"

using namespace std;
// ���������� ������ ��� ��������?������??����?��������?
// ���������? ������? ������??���� ����������?
// ��?���������� ��������? ��������� ?���� "cpp_errors.txt"

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
   
    MKL_INT sorder = DF_PP_CUBIC;       // ��?��?������?(spline order) 
    MKL_INT stype = DF_PP_NATURAL;      // ��?������?(spline type)

    MKL_INT nx = n;                     // ����?����?������?(number of break points)
    MKL_INT xhint = DF_NON_UNIFORM_PARTITION;  // ������? ������?���������? �� ������������?����?
    //double xUni[2];    // ��� ����������?����?����?�������� ������ ������?��������?��������������? 
    //xUniform[0] = xL;
    //xUniform[1] = xR;
  
    MKL_INT ny = 1;                     // ������ ������-������?(number of functions)
    MKL_INT yhint = DF_NO_HINT;         // ������������? ��������? �� ����������?

    MKL_INT nscoeff = ny * (nx - 1) * DF_PP_CUBIC;   // ����?������������?������?(number of spline coefficients)
    MKL_INT scoeffhint = DF_NO_HINT;                  // ������������? ��������? �� ����������?
    double* scoeff = new double[nscoeff];             // ������ ������������?������?(array of spline coefficients)

    MKL_INT bc_type = DF_BC_1ST_LEFT_DER | DF_BC_1ST_RIGHT_DER; // ��?��������?������?
    double* bc = new double[2]{ derLeft, derRight };   // ������? ����������?�� ������ ������?
 
    MKL_INT ic_type = DF_NO_IC;     // ��?������?�� ���������� ������ (internal conditions type)
    double* ic;                     // ������� �� ���������� ������ (internal conditions)
    ic = 0;                         // ������� �� ���������� ������ �� ����������?

    DFTaskPtr task;                     // �������� ����������?(Data Fitting task descriptor)
    int errcode = dfdNewTask1D(&task, nx, x, xhint, ny, y, yhint);
    ofs << "dfdNewTask1D errcode = " << errcode << endl;
  
    errcode = dfdEditPPSpline1D(task, sorder, stype, bc_type, bc, ic_type, ic, scoeff, scoeffhint);
    ofs << "dfdEditPPSpline1D errcode = " << errcode << endl;
   
    errcode = dfdConstruct1D(task, DF_PP_SPLINE, DF_METHOD_STD);
    ofs << "dfdConstruct1D errcode = " << errcode << endl;
    MKL_INT nsite = nN;
   
    MKL_INT dorder[3]{ 1 , 1 , 1 };  // ���������? ������? ������? ������ ?������ ����������?
  
    // ���������� �������� ������??���� ����������??������ site
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
    ofs.close();     // ��������?����
}