pipeline {
    agent any 

    environment { 
        APP_VERSION = 'v1.0.0'
        DEV_PREFIX = 'dev.'
        DOCKER_NAME_TEST_PREFIX = 'test.'        
    }

    stages {  
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
               sh 'docker run -d --name ${NAME} -p ${PORT}:80 ${IMAGE_NAME}'
            }
        }  
        

        stage("setup nginx - dev"){
            when {
                changeset "nginx/*"
                branch 'develop'
            }

            environment {
                NAME = "${DEV_PREFIX}" + "ngx"             
                IMAGE_NAME = "${NAME}:" + "${APP_VERSION}" + "." + "${BUILD_ID}"
                PORT = '8800'
            }

            steps {
                sh 'docker build -t ${IMAGE_NAME} -f nginx/nginx.config.dev .'
                sh 'docker rm -f ${NAME} || true'
                sh 'docker run -d --name ${NAME} -p ${PORT}:80 ${IMAGE_NAME}'
            }
        }
    }
}