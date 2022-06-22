#!/bin/bash
docker stop test_env
docker rm test_env
docker rmi uniimp_test_env 
docker build -t uniimp_test_env .
docker create --name test_env uniimp_test_env
docker start test_env