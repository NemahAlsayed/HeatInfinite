#!/bin/bash

# Find and delete duplicate AssemblyInfo files
find /workspaces/HeatInfinite/obj/net8.0/ -type f -name "*.AssemblyInfo.cs" -exec rm -f {} +

# Remove any duplicate .NETCoreApp attributes files
find /workspaces/HeatInfinite/obj/net8.0/ -type f -name "*.NETCoreApp,Version=v8.0.AssemblyAttributes.cs" -exec rm -f {} +

# Clear old build directories
rm -rf /workspaces/HeatInfinite/obj /workspaces/HeatInfinite/bin

echo "Duplicate files removed. Run 'dotnet restore && dotnet build' now!"
