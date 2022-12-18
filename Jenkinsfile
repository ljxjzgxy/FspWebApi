pipeline {
    agent any 

    environment { 
        APP_VERSION = 'v1.0.0'
        DEV_PREFIX = 'dev.'
        DOCKER_NAME_TEST_PREFIX = 'test.'    
        DEV_MICROSERVICE_NETWORK = 'dev_microservice'
    }

    stages {  
        stage('create microserver network - dev'){            
            steps{
                sh 'docker network create ${DEV_MICROSERVICE_NETWORK} || true'
            }
        }
        
        stage('build identify.svc - dev') {
            when { 
                changeset "identify.svc/**"
                branch 'develop'
            }

            environment {
                NAME = "${DEV_PREFIX}" + "identify.svc"                
                IMAGE_NAME = "${NAME}:" + "${APP_VERSION}" + "." + "${BUILD_ID}"
                PORT = '8888'
            }

            steps {
               sh 'docker build -t ${IMAGE_NAME} -f identify.svc/Dockerfile.dev .'
               sh 'docker rm -f ${NAME} || true'
               sh 'docker run -d --name ${NAME} --network ${DEV_MICROSERVICE_NETWORK} -p ${PORT}:80 ${IMAGE_NAME}'
            }
        }  
        

        stage("setup nginx - dev"){
            when {
                branch 'develop'
            }

            environment {
                NAME = "${DEV_PREFIX}" + "ngx"             
                IMAGE_NAME = "${NAME}:" + "${APP_VERSION}" + "." + "${BUILD_ID}"
                PORT = '8800'
            }

            steps {
                sh 'docker build -t ${IMAGE_NAME} -f nginx/docker.nginx.dev .'
                sh 'docker rm -f ${NAME} || true'
                sh 'docker run -d --name ${NAME} --network ${DEV_MICROSERVICE_NETWORK} -p ${PORT}:80 ${IMAGE_NAME}'
            }
        }
    }
}