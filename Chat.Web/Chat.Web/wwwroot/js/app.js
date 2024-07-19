const chatUrl = "https://localhost:20001/chat";
let _connection, _token;

login();
connect();
document.getElementById("disconnect").addEventListener("click", disconnect);
document.getElementById("send").addEventListener("click", send);

let _users = [];

async function start() {
    try {
        await _connection.start();
        console.log(`SignalR Connected to ${chatUrl}`);
    } catch (err) {
        console.log(err);
        setTimeout(start, 5000);
    }
};

function buildConnection() {
    _connection = new signalR.HubConnectionBuilder()
        .withUrl(chatUrl,
            {
                accessTokenFactory: () => _token
            })
        .configureLogging(signalR.LogLevel.Information)
        .build();

    _connection.on("UpdateUsersAsync", (users) => {
        _users = users;
        const userList = document.getElementById("userList");
        while (userList.firstChild) {
            userList.removeChild(userList.lastChild);
        }

        const ul = document.createElement("ul");
        for (let i = 0; i < _users.length; i++) {
            const li = document.createElement("li");
            li.textContent = _users[i];
            ul.appendChild(li);
        }
        userList.appendChild(ul);
    });

    _connection.onclose(async () => {
        document.getElementById("connect").removeAttribute("disabled");
        document.getElementById("disconnect").setAttribute("disabled", "");
        console.log(`SignalR now ${_connection.state}`);
    });

    _connection.on("SendMessageAsync", (user, message) => {
        const li = document.createElement("li");
        li.textContent = `${user}: ${message}`;
        document.getElementById("messageList").appendChild(li);
    });
}

function login() {
   
    const urlParams = new URLSearchParams(window.location.search);
   
    _token = urlParams.get('token');
    console.log(_token);
   // document.getElementById("token").innerHTML = _token;
    if (_token == null) {
        document.getElementById("token").innerHTML = "token error";
    }              
    document.getElementById("userToolbar").classList.remove("invisible");
    
    return;
}

function send() {
    var message = document.getElementById("messageText").value;
    const urlParams = new URLSearchParams(window.location.search);

    const userName = urlParams.get('username');
    console.log(_token);
      //document.getElementById("username").value;
    if (!message || !userName) {
        alert("Message and Username is required");
        return;
    }

    _connection.invoke("SendMessageAsync", userName, message);
}

function connect() {

    if (!_token) {
        const li = document.createElement("li");
        li.textContent = "No token found";
        document.getElementById("messageList").appendChild(li);
    }

    // Start the connection.
    start();

    document.getElementById("disconnect").removeAttribute("disabled");
    document.getElementById("connect").setAttribute("disabled", "");
    document.getElementById("send-form").classList.remove("invisible");

    return;
}
function disconnect() {

    if (_connection && _connection.state === 'Connected') {
        console.log("Disconnecting...");
        _connection.stop();

        // Clear token
        document.getElementById("send-form").classList.add("invisible");
    }
    return;
}

// build connection
buildConnection();