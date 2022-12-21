pipeline {
    agent any 

    environment { 
        APP_VERSION = 'v1.1.0'
        DEV_MICROSERVICE_NETWORK = 'dev_microservice'

        DEV = 'dev'
        DEV_PREFIX = "${DEV}" + "."
        DOCKER_FILE_DEV = "Dockerfile.dev"

        TEST = "test"
        TEST_PREFIX = "${TEST}" + '.'    
       
        APP_SETTINGS_DIR_ROOT = "/etc/aspnetcore_appsettings"

        APPSETTTINGS_JSON = "appsettings.json"
        APPSETTTINGS_DEV_JSON = "appsettings.Development.json"
        APPSETTINGS_TEST_JSON = "appsettings.Test.json"
    }

    stages {       
        stage('create microserver network - dev'){            
            steps{
                sh 'docker network create ${DEV_MICROSERVICE_NETWORK} || true'
            }
        }
        
        stage('build identity.svc - dev') {
            environment {
                SVC_NAME = "identity.svc"
                CONTAINER_NAME = "${DEV_PREFIX}" + "${SVC_NAME}"                
                IMAGE_NAME = "${CONTAINER_NAME}:" + "${APP_VERSION}" + "." + "${BUILD_ID}"
                PORT = '8888'
            }

            when { 
                changeset "identity.svc/**"
                branch 'develop'
            }           

            steps {
                 sh '''
                    APPSETTINGS_JSON_FILE="${APP_SETTINGS_DIR_ROOT}/${DEV}/${SVC_NAME}/${APPSETTTINGS_JSON}"
                    if [ -f "$APPSETTINGS_JSON_FILE" ]; then
                        echo "File ${APPSETTINGS_JSON_FILE} found..."
                    else
                        echo "Error: File ${APPSETTINGS_JSON_FILE} not found. Can not continue."
                        exit 1
                    fi

                    APPSETTINGS_DEV_JSON_FILE="${APP_SETTINGS_DIR_ROOT}/${DEV}/${SVC_NAME}/${APPSETTTINGS_DEV_JSON}"
                    if [ ! -f "$APPSETTINGS_DEV_JSON_FILE" ]; then
                      echo "Error: File ${APPSETTINGS_JSON_FILE} not found. Can not continue."
                      exit 1
                    else
                        echo "File ${APPSETTINGS_DEV_JSON_FILE} found..."
                    fi

                    cp $APPSETTINGS_JSON_FILE ./${SVC_NAME}
                    cp $APPSETTINGS_DEV_JSON_FILE ./${SVC_NAME}

                    docker build -t ${IMAGE_NAME} -f ${SVC_NAME}/${DOCKER_FILE_DEV} .
                    docker rm -f ${CONTAINER_NAME} || true
                    docker run -d --name ${CONTAINER_NAME} --network ${DEV_MICROSERVICE_NETWORK} -p ${PORT}:80 ${IMAGE_NAME}
                '''             
            }
        }  
        

        stage("setup nginx - dev"){
            when {
                branch 'develop'
            }

            environment {
                CONTAINER_NAME = "${DEV_PREFIX}" + "ngx"             
                IMAGE_NAME = "${CONTAINER_NAME}:" + "${APP_VERSION}" + "." + "${BUILD_ID}"
                PORT = '8800'
            }

            steps {
                sh '''
                    docker build -t ${IMAGE_NAME} -f nginx/docker.nginx.dev .
                    docker rm -f ${CONTAINER_NAME} || true
                    docker run -d --name ${CONTAINER_NAME} --network ${DEV_MICROSERVICE_NETWORK} -p ${PORT}:80 ${IMAGE_NAME}
                ''' 
            }
        }       
    }

    post{
        always{
            sh 'docker image prune --all --filter until=48h -f'
        }
    }
}