


pipeline {
    agent any 

    environment { 
        DOCKER_IMAGE_VERSION = 'v1.0.0'
        DOCKER_NAME_DEV_PREFIX = 'dev.'
        DOCKER_NAME_TEST_PREFIX = 'test.'
        PORT = '8888'
    }

    stages {  
        stage('build identify.svc - dev') {
            when { 
                changeset "identify.svc/**"
                branch 'develop'
            }

            environment {
                NAME = '${DOCKER_NAME_DEV_PREFIX}identify.svc'                
                IMAGE_NAME = '${NAME}:${DOCKER_IMAGE_VERSION}.${BUILD_ID}'
            }

            steps {
               sh 'docker build -t $IMAGE_NAME -f identify.svc/Dockerfile.dev .'
               sh 'docker rm -f $NAME || true'
               sh 'docker run -d --name $NAME -p $PORT:80 $IMAGE_NAME'
            }
        }   
    }
}