﻿namespace GEH.Exceptions;

public class NotFoundException(string errorMessage = "Item not found") 
    : Exception(errorMessage)
{ }