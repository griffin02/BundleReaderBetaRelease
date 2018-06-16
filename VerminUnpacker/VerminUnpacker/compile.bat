@echo off

del /Q Bundle.o BundleReader.o BundleException.o Convert.o

g++ -c Convert.cpp -o Convert.o -O2 --std=c++11
g++ -c Bundle.cpp -o Bundle.o -O2 --std=c++11
g++ -c BundleReader.cpp -o BundleReader.o -O2 --std=c++11
g++ -c BundleException.cpp -o BundleException.o -O2 --std=c++11
g++ main.cpp Bundle.o BundleReader.o BundleException.o Convert.o zlibwapi.dll -o main.exe -I "../../tools/zlib" -static -O2 --std=c++11

pause

time < nul

main.exe

time < nul

pause