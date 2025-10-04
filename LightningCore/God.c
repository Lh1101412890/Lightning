#include <malloc.h>
#include <shlobj.h>
#include <stdio.h>
#include <stdlib.h>

typedef struct LPoint
{
	double X;
	double Y;
}LPoint;

typedef struct LPolyline
{
	LPoint* POINT;
	int NumberOfVertices;
}LPolyline;

int IsInside(LPoint point, LPolyline polyline) {
	double minx = 0, miny = 0, maxx = 0, maxy = 0;
	for (int i = 0; i < polyline.NumberOfVertices; i++)
	{
		if ((polyline.POINT + i)->X < minx) {
			minx = (polyline.POINT + i)->X;
		}
		if ((polyline.POINT + i)->Y < miny) {
			miny = (polyline.POINT + i)->Y;
		}
		if ((polyline.POINT + i)->X > maxx) {
			maxx = (polyline.POINT + i)->X;
		}
		if ((polyline.POINT + i)->Y > maxy) {
			maxy = (polyline.POINT + i)->Y;
		}
	}
	if (point.X >= minx && point.X <= maxx && point.Y >= miny && point.Y <= maxy) {
		if (polyline.NumberOfVertices > 4) {

		}
		return 1;
	}
	else
	{
		return -1;
	}
}

/// <summary>
/// 返回2024-7-5
/// </summary>
/// <returns></returns>
__declspec(dllexport) TCHAR* GetDate() {
	TCHAR* date = L"2024-7-5";
	return date;
}

/// <summary>
/// 使用期限xml数据,初始时长15天
/// </summary>
/// <returns></returns>
__declspec(dllexport) TCHAR* GetData() {
	TCHAR* data = L"<?xml version = \"1.0\" encoding=\"utf-8\" ?>\r\n<!DOCTYPE AppInfos[\r\n\t<!ELEMENT AppInfos         (Logs,UserInfo)>\r\n\t<!ELEMENT Logs\t\t\t   (Log*)>\r\n\t<!ELEMENT UserInfo\t\t   (IsForever,Time,LastTime)>\r\n\t<!ELEMENT Log\t\t\t   (#PCDATA)>\r\n\t<!ELEMENT IsForever\t\t   (#PCDATA)>\r\n\t<!ELEMENT Time             (#PCDATA)>\r\n\t<!ELEMENT LastTime         (#PCDATA)>\r\n]>\r\n\r\n<AppInfos>\r\n\t<Logs>\r\n\t</Logs>\r\n\t<UserInfo>\r\n\t\t<IsForever>false</IsForever>\r\n\t\t<Time>1296000</Time>\r\n\t\t<LastTime></LastTime>\r\n\t</UserInfo>\r\n</AppInfos>";
	return data;
}

/// <summary>
/// 获取时限文件路径
/// </summary>
/// <param name="product">产品代号：CAD、Revit、Office</param>
/// <returns>时限文件路径</returns>
__declspec(dllexport) TCHAR* GetTimeFile(TCHAR* product) {
	TCHAR szPath[MAX_PATH];
	SHGetFolderPath(NULL, CSIDL_COMMON_DOCUMENTS, NULL, SHGFP_TYPE_CURRENT, szPath);

	int length1 = 0;
	while (TRUE) {
		if (szPath[length1] != L'\0') {
			length1++;
		}
		else
		{
			break;
		}
	}
	int length2 = sizeof(L"\\00e7845d7d2726f31c0161c3ed2fad6a.xml");
	TCHAR* pa = (TCHAR*)malloc(sizeof(TCHAR) * (length1 + length2));
	if (pa == NULL) {
		return NULL;
	}
	for (int i = 0; i < length2; i++)
	{
		*(pa + i) = szPath[i];
	}
	TCHAR* cad = L"\\00e7845d7d2726f31c0161c3ed2fad6a.xml";
	TCHAR* revit = L"\\954d57fdafb5471f0d74865ef55567bd.xml";
	TCHAR* office = L"\\04dd80b3a4aef906b04ceeda236f2347.xml";

	if (wcscmp(product, L"CAD") == 0) {
		for (int i = 0; i < length2; i++)
		{
			*(pa + length1 + i) = *(cad + i);
		}
	}
	if (wcscmp(product, L"Revit") == 0) {
		for (int i = 0; i < length2; i++)
		{
			*(pa + length1 + i) = *(revit + i);
		}
	}
	if (wcscmp(product, L"Office") == 0) {
		for (int i = 0; i < length2; i++)
		{
			*(pa + length1 + i) = *(office + i);
		}
	}
	return pa;
}

/// <summary>
/// 释放由 malloc 或相关函数分配的内存。
/// </summary>
/// <param name="p">指向要释放的内存块的指针。</param>
__declspec(dllexport) void Free(void* p) {
	free(p);
}