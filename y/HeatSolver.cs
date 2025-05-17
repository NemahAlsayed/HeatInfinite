public static class HeatSolver
{
    public static double[,] SolveHeatEquation(double dx, double dy)
    {
        int nx = (int)(120 / dx) + 1;
        int ny = (int)(100 / dy) + 1;
        double[,] temperature = new double[ny, nx];

        for (int j = 0; j < ny; j++)
        {
            for (int i = 0; i < nx; i++)
            {
                temperature[j, i] = 25.0; // Initial condition
                if (i == 0) temperature[j, i] += 50.0 * dx / 1.0;
                if (i == nx - 1) temperature[j, i] += 25.0 * dx / 1.0;
                if (j == 0) temperature[j, i] = 25.0;
                if (j == ny - 1) temperature[j, i] = 100.0;
            }
        }

        return temperature;
    }
}
