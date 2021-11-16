db.createUser({
    user: "root",
    pwd: "devry123",
    roles: [
        {
            role: "readWrite",
            db: "engineersnotebook"
        }
    ]
})