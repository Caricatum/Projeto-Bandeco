package com.example.apiBandeco.exception;

import org.springframework.web.bind.MethodArgumentNotValidException;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.server.ResponseStatusException;

import java.util.HashMap;
import java.util.Map;

@RestControllerAdvice
public class TratadorErros {

    @ExceptionHandler(MethodArgumentNotValidException.class)
    public Map<String, String> tratarErros(MethodArgumentNotValidException ex) {
        Map<String, String> erros = new HashMap<>();

        ex.getBindingResult().getFieldErrors().forEach(error -> {
            erros.put(error.getField(), error.getDefaultMessage());
        });

        return erros;
    }

    @ExceptionHandler(ResponseStatusException.class)
    public Map<String, String> tratarResponseStatus(ResponseStatusException ex) {
        Map<String, String> erro = new HashMap<>();

        erro.put("erro", ex.getReason());

        return erro;
    }
}
