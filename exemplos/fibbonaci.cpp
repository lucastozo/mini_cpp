#include <iostream>
using namespace std;

int main()
{
    double a, b, c;
    a = 0; b = 1;
    double limite;
    limite = 10;
    while (limite > 0)
    {
        c = a + b;
        a = b;
        b = c;
        limite = limite - 1;
    }
    cout << a << endl;
}