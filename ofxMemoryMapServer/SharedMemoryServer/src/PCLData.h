#pragma once

#define NUM_POINTS 1000

struct Vec3
{
	float x;
	float y;
	float z;
};

struct Color
{
	float r;
	float g;
	float b;
	float a;
};

struct PCLPoint
{
	Vec3 pos;
	Color color;
};

struct PCLData {
	PCLPoint points[NUM_POINTS];
};

