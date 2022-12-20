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
            environment {
                SVC_NAME = "identify.svc"
                NAME = "${DEV_PREFIX}" + "${SVC_NAME}"                
                IMAGE_NAME = "${NAME}:" + "${APP_VERSION}" + "." + "${BUILD_ID}"
                PORT = '8888'
            }

            when { 
                changeset "identify.svc/**"
                branch 'develop'
            }           

            steps {
                 sh '''
                    APPSETTINGS_JSON_FILE="/etc/aspnet_appsettings/${SVC_NAME}/appsettings.json"
                    if [ -f "$APPSETTINGS_JSON_FILE" ]; then
                        echo "File ${APPSETTINGS_JSON_FILE} found..."
                    else
                        echo "Error: File ${APPSETTINGS_JSON_FILE} not found. Can not continue."
                        exit 1
                    fi

                    APPSETTINGS_DEV_JSON_FILE="/etc/aspnet_appsettings/${SVC_NAME}/appsettings.Development.json"
                    if [ ! -f "$APPSETTINGS_DEV_JSON_FILE" ]; then
                      echo "Error: File ${APPSETTINGS_JSON_FILE} not found. Can not continue."
                      exit 1
                    else
                        echo "File ${APPSETTINGS_DEV_JSON_FILE} found..."
                    fi

                    cp $APPSETTINGS_JSON_FILE ./{SVC_NAME}
                    cp $APPSETTINGS_DEV_JSON_FILE ./{SVC_NAME}

                    docker build -t ${IMAGE_NAME} -f ${SVC_NAME}/Dockerfile.dev .
                    docker rm -f ${NAME} || true
                    docker run -d --name ${NAME} --network ${DEV_MICROSERVICE_NETWORK} -p ${PORT}:80 ${IMAGE_NAME}
                '''             
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
                sh '''
                    docker build -t ${IMAGE_NAME} -f nginx/docker.nginx.dev .
                    docker rm -f ${NAME} || true
                    docker run -d --name ${NAME} --network ${DEV_MICROSERVICE_NETWORK} -p ${PORT}:80 ${IMAGE_NAME}
                ''' 
            }
        }
    }
}