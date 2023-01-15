import React, { useState, useEffect } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';

function SignalRTest() {

    const [connection, setConnection] = useState(null);
    const [conStatus, setConStatus] = useState("");
    const [receivedMsg, setReceivedMsg] = useState(null);
    const [refreshCount, setRefreshCount] = useState(0);

    useEffect(() => {
        const con = new HubConnectionBuilder()
            .withUrl('http://localhost:5009/servicesMonitorHub')
            .withAutomaticReconnect()
            .build();

        setConnection(con);
        setConStatus("Building SignalR connection...");
    }, []);

    useEffect(() => {
        if (connection) {
            connection.start()
                .then(_ => {
                    setConStatus('Connected!');
                    connection.on('ClientServicesMonitorDataUpdate', objMsg => {
                        setRefreshCount(pre => pre + 1)
                        setReceivedMsg(objMsg)
                    });
                })
                .catch(e => {
                    setConStatus(`Connection failed`)
                    console.log("signalR error---------->", e)
                });
        }
    }, [connection]);

    return (
        <>
            <h1>{conStatus}</h1>
            <hr></hr>
            <h2>SignalR Message: (refresh: {refreshCount}) </h2>
            {
                receivedMsg && receivedMsg.map(msg =>
                    <div style={{ marginLeft: 100 }} key={msg.id}>
                        <h3>{msg.serviceName}</h3>
                        <pre>{JSON.stringify(msg, null, 5)} </pre>
                    </div>
                )
            }
        </>
    );
}

export default SignalRTest;