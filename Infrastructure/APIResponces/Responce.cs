using System;

namespace Infrasrtucture.APIResponces;



public class Responce<T>
{
    
    public bool IsSucced { get; set; }
    public int StatusCode { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }

    public static Responce<T> Ok(T? data,string? message=null)
    {
        return new Responce<T>
        {
            IsSucced = true,
            StatusCode = 200,
            Message = message,
            Data = data
        };
    }
    public static Responce<T> Created(string? message, T? data = default)
    {
        return new Responce<T>
        {
            IsSucced = true,
            StatusCode = 201,
            Message = message,
            Data = data
        };
    }
    public static Responce<T> Fail(int statusCode, string message)
    { 
        return new Responce<T>
        {
            IsSucced = false,
            StatusCode = 201,
            Message = message,
            Data = default
        };
    }
}


