define({ "api": [
  {
    "type": "post",
    "url": "/Auth/AddToRole",
    "title": "AddToRole",
    "version": "0.1.1",
    "name": "AddToRole",
    "group": "Auth",
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "GUID",
            "optional": false,
            "field": "UserID",
            "description": "<p>ID of user</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "roleName",
            "description": "<p>Name of role</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "status",
            "description": "<p>Status of successful response</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n{\n  \"status\":\"User added to role Customer\"\n}",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "RoleNotFound",
            "description": "<p>Role name is invalid.</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "UserError",
            "description": "<p>User cannot be added to role.</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "UserNotExists",
            "description": "<p>User cannot be found.</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 400 BadRequest\n{\n \"status\":\"Role does not exists\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 400 BadRequest\n{\n     \"status\":User cannot be added to role Customer\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 400 BadRequest\n{\n     \"status\":User does not exists\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "./Controllers/AuthController.cs",
    "groupTitle": "Auth"
  },
  {
    "type": "post",
    "url": "/Auth/CheckIn",
    "title": "CheckIn",
    "version": "0.1.0",
    "name": "CheckIn",
    "group": "Auth",
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "emailOrLogin",
            "description": "<p>Email or login of user to be checked in</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "roomNumber",
            "description": "<p>Number of room to which user will be checked in</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "status",
            "description": "<p>User succesfully checked in</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n    {\n    \"status\":\"checkedIn\"\n    }",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "InvalidRole",
            "description": "<p>Only user with role Customer can occupy room</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "UserAlreadyCheckedIn",
            "description": "<p>One customer can occupy only one room at the same time</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "RoomAlreadyOccupied",
            "description": "<p>If specified room is already occupied, validation fails</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "InvalidInput",
            "description": "<p>If Room or User cannot be found</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 200 OK\n{ \n    \"status\":\"invalidRole\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 200 OK\n{\n   \"status\":\"userAlreadyCheckedIn\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 200 OK\n{\n   \"status\":\"roomAlreadyOccupied\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 200 OK\n{ \n   \"status\":\"invalidInput\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "./Controllers/AuthController.cs",
    "groupTitle": "Auth"
  },
  {
    "type": "post",
    "url": "/Auth/CheckOut",
    "title": "CheckOut",
    "version": "0.1.0",
    "name": "CheckOut",
    "group": "Auth",
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "emailOrLogin",
            "description": "<p>Email or login of user to be checked out</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "roomNumber",
            "description": "<p>For verification - number of room that is occupied by requested user</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "status",
            "description": "<p>User succesfully checked out</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n    {\n    \"status\":\"checkedOut\"\n    }",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "InvalidRole",
            "description": "<p>Only user with role Customer can occupy room</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "UserAlreadyCheckedOut",
            "description": "<p>If user does not occupy specified room, validation fails</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "RoomNotOccupied",
            "description": "<p>If specified room is not occupied, validation fails</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "InvalidInput",
            "description": "<p>If Room or User cannot be found</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 200 OK\n{ \n    \"status\":\"invalidRole\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 200 OK\n{\n   \"status\":\"userAlreadyCheckedOut\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 200 OK\n{\n   \"status\":\"roomNotOccupied\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 200 OK\n{ \n   \"status\":\"invalidInput\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "./Controllers/AuthController.cs",
    "groupTitle": "Auth"
  },
  {
    "deprecated": {
      "content": "do not use it."
    },
    "type": "get",
    "url": "/Auth/Login",
    "title": "Login - clear cookie",
    "version": "0.1.1",
    "name": "GetLogin",
    "group": "Auth",
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "status",
            "description": "<p>Message about cleared cookie.</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n{\n  \"status\": \"clear\",\n}",
          "type": "json"
        }
      ]
    },
    "filename": "./Controllers/AuthController.cs",
    "groupTitle": "Auth"
  },
  {
    "type": "post",
    "url": "/Auth/GetUserRoles",
    "title": "GetUserRoles",
    "version": "0.1.1",
    "name": "GetUserRoles",
    "group": "Auth",
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "GUID",
            "optional": false,
            "field": "UserID",
            "description": "<p>ID of user</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "List",
            "optional": false,
            "field": "roles",
            "description": "<p>Roles of user</p>"
          }
        ]
      }
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "UserNotExists",
            "description": "<p>User cannot be found.</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 400 BadRequest\n{\n     \"status\":User does not exists\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "./Controllers/AuthController.cs",
    "groupTitle": "Auth"
  },
  {
    "type": "post",
    "url": "/Auth/Logout",
    "title": "Logout",
    "version": "0.1.0",
    "name": "Logout",
    "group": "Auth",
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "status",
            "description": "<p>User succesfully logged out</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n    {\n    \"status\":\"logout\"\n    }",
          "type": "json"
        }
      ]
    },
    "filename": "./Controllers/AuthController.cs",
    "groupTitle": "Auth"
  },
  {
    "deprecated": {
      "content": "use now (#Auth:Token)."
    },
    "type": "post",
    "url": "/Auth/Login",
    "title": "Login",
    "version": "0.1.1",
    "name": "PostLogin",
    "group": "Auth",
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "Login",
            "description": "<p>Email or login of user</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "Password",
            "description": "<p>User's password</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "FirstName",
            "description": "<p>First name of user.</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "LastName",
            "description": "<p>Last name of user.</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "Email",
            "description": "<p>Optional email address of user</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "PhoneNumber",
            "description": "<p>Optional phone number of user</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "WorkerType",
            "description": "<p>One of available types (Cleaner,Technician,None).</p>"
          },
          {
            "group": "Success 200",
            "type": "Array",
            "optional": false,
            "field": "Role",
            "description": "<p>All roles that particular user have</p>"
          },
          {
            "group": "Success 200",
            "type": "GUID",
            "optional": false,
            "field": "RoomID",
            "description": "<p>Optional parameter - only guests have this not-null</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n{\n\"firstName\":\"Abraham\",\n\"lastName\":\"Lincoln\",\n\"email\":\"president@usa.pl\",\n\"phoneNumber\":\"123-908-123\",\n\"workerType\":\"Technician\",\n\"role\":[\"Worker\"]\n}",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "MissingData",
            "description": "<p>Login or Password are missing.</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "Unathorized",
            "description": "<p>This User does not exist or password is invalid.</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 200 OK\n{\n \"status\":\"fail\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 200 OK\n{\n    \"status\":\"unauthorized\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "./Controllers/AuthController.cs",
    "groupTitle": "Auth"
  },
  {
    "type": "post",
    "url": "/Auth/Register",
    "title": "Register",
    "version": "0.1.0",
    "name": "Register",
    "group": "Auth",
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "Login",
            "description": "<p>Login of user</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "Password",
            "description": "<p>Password of user</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "FirstName",
            "description": "<p>Optional user name</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "LastName",
            "description": "<p>Optional user surname</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "PhoneNumber",
            "description": "<p>Optional contact number</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "RoleName",
            "description": "<p>One of roles (Worker,Customer,Manager,Administrator)</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "Email",
            "description": "<p>Required user identifier</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "WorkerType",
            "description": "<p>One of available types (Cleaner,Technician,None)</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "status",
            "description": "<p>User succesfully created</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n    {\n    \"status\":\"registered\"\n    }",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "InvalidInput",
            "description": "<p>One of entries is not valid</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "EmailNotUnique",
            "description": "<p>Email address have to be unique</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "RoleNameInvalid",
            "description": "<p>Role Name must be valid with existing roles</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 200 OK\n{ \n    \"status\":\"failure\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 200 OK\n{ \n   \"status\":\"userIdentifierNotUnique\"\n   }",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 200 OK\n{\n   \"status\":\"roleNameInvalid\"\n   }",
          "type": "json"
        }
      ]
    },
    "filename": "./Controllers/AuthController.cs",
    "groupTitle": "Auth"
  },
  {
    "type": "post",
    "url": "/Auth/Token",
    "title": "Token",
    "version": "0.1.1",
    "name": "TokenLogin",
    "group": "Auth",
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "Login",
            "description": "<p>Email or login of user</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "Password",
            "description": "<p>User's password</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "FirstName",
            "description": "<p>First name of user.</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "LastName",
            "description": "<p>Last name of user.</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "Email",
            "description": "<p>Optional email address of user</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "WorkerType",
            "description": "<p>One of available types (Cleaner,Technician,None).</p>"
          },
          {
            "group": "Success 200",
            "type": "Array",
            "optional": false,
            "field": "Role",
            "description": "<p>All roles that particular user have</p>"
          },
          {
            "group": "Success 200",
            "type": "GUID",
            "optional": false,
            "field": "RoomID",
            "description": "<p>Optional parameter - only guests have this not-null</p>"
          },
          {
            "group": "Success 200",
            "type": "Token",
            "optional": false,
            "field": "token",
            "description": "<p>Authentication token that should be send in every response as header (headerKey:Authenticate, headerValue: &quot;bearer &quot; + token)</p>"
          },
          {
            "group": "Success 200",
            "type": "DateTime",
            "optional": false,
            "field": "expiration",
            "description": "<p>Date when token expires</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n{\n\"firstName\":\"Abraham\",\n\"lastName\":\"Lincoln\",\n\"email\":\"president@usa.pl\",\n\"workerType\":\"Technician\",\n\"role\":[\"Worker\"]\n\"token\":\"blablabla121212\",\n\"expiration\":\"2017-05-07T20:49:48Z\"\n}",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "LoginFailed",
            "description": "<p>Password is invalid.</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "InternalError",
            "description": "<p>This User does not exist.</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 400 BadRequest\n{\n \"status\":\"failedToLogin\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 404 BadRequest\n{\n    \"status\":\"failed\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "./Controllers/AuthController.cs",
    "groupTitle": "Auth"
  },
  {
    "type": "post",
    "url": "/Case",
    "title": "Create",
    "version": "0.1.0",
    "name": "Create",
    "group": "Case",
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "Title",
            "description": "<p>Case title</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "Description",
            "description": "<p>Case details</p>"
          },
          {
            "group": "Parameter",
            "type": "Number",
            "optional": false,
            "field": "WorkerType",
            "description": "<p>Type of case - one of (0-Cleaner,1-Technician,2-None)</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "status",
            "description": "<p>Case was created</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n      {\n      \"status\":\"created\"\n      }",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "InvalidInput",
            "description": "<p>One of inputs was null or invalid</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 200 OK\n{\n  \"status\":\"failure\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "./Controllers/CaseController.cs",
    "groupTitle": "Case"
  },
  {
    "type": "delete",
    "url": "/Case/CaseID",
    "title": "Delete",
    "version": "0.1.0",
    "name": "Delete",
    "group": "Case",
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "GUID",
            "optional": false,
            "field": "CaseID",
            "description": "<p>Case identifier</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "status",
            "description": "<p>Case was deleted</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n      {\n      \"status\":\"removed\"\n      }",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "NotFound",
            "description": "<p>Case with specified ID was not found</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 200 OK\n{\n \"status\":\"notFound\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "./Controllers/CaseController.cs",
    "groupTitle": "Case"
  },
  {
    "type": "get",
    "url": "/Case",
    "title": "List",
    "version": "0.1.0",
    "name": "List",
    "group": "Case",
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "Array",
            "optional": false,
            "field": "cases",
            "description": "<p>List of all existing cases</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n  [\n      {\n      \"CaseID\":\"4ba83f3c-4ea4-4da4-9c06-e986a8273800\",\n      \"Title\":\"ExampleCase\",\n      \"Description\":\"Clean something\",\n      \"WorkerType\":\"Technician\"\n      }\n  ]",
          "type": "json"
        }
      ]
    },
    "filename": "./Controllers/CaseController.cs",
    "groupTitle": "Case"
  },
  {
    "type": "get",
    "url": "/Case/Filter/WorkerType",
    "title": "Read",
    "version": "0.1.0",
    "name": "Read",
    "group": "Case",
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "WorkerType",
            "description": "<p>(Cleaner or Technician)</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "CaseID",
            "description": "<p>Case identifier</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "Title",
            "description": "<p>Case title</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "Description",
            "description": "<p>Case details</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "WorkerType",
            "description": "<p>Worker type associated with this case</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n      {\n      \"CaseID\":\"4ba83f3c-4ea4-4da4-9c06-e986a8273800\",\n      \"Title\":\"ExampleCase\",\n      \"Description\":\"Clean something\",\n      \"WorkerType\":\"Technician\"\n      }",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "NotFound",
            "description": "<p>Given ID does not appeal to any of cases</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 200 OK\n{\n  \"status\":\"notFound\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "./Controllers/CaseController.cs",
    "groupTitle": "Case"
  },
  {
    "type": "get",
    "url": "/Case/CaseID",
    "title": "Read",
    "version": "0.1.0",
    "name": "Read",
    "group": "Case",
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "GUID",
            "optional": false,
            "field": "CaseID",
            "description": "<p>Case identifier</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "CaseID",
            "description": "<p>Case identifier</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "Title",
            "description": "<p>Case title</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "Description",
            "description": "<p>Case details</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "WorkerType",
            "description": "<p>Worker type associated with this case</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n      {\n      \"CaseID\":\"4ba83f3c-4ea4-4da4-9c06-e986a8273800\",\n      \"Title\":\"ExampleCase\",\n      \"Description\":\"Clean something\",\n      \"WorkerType\":\"Technician\"\n      }",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "NotFound",
            "description": "<p>Given ID does not appeal to any of cases</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 200 OK\n{\n  \"status\":\"notFound\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "./Controllers/CaseController.cs",
    "groupTitle": "Case"
  },
  {
    "type": "put",
    "url": "/Case?CaseID",
    "title": "Update",
    "version": "0.1.0",
    "name": "Update",
    "group": "Case",
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "GUID",
            "optional": false,
            "field": "CaseID",
            "description": "<p>Case identifier</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "Title",
            "description": "<p>Case title</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "Description",
            "description": "<p>Case details</p>"
          },
          {
            "group": "Parameter",
            "type": "Number",
            "optional": false,
            "field": "WorkerType",
            "description": "<p>Type of case - one of (0-Cleaner,1-Technician,2-None)</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "status",
            "description": "<p>Case was updated</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n      {\n      \"status\":\"updated\"\n      }",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "InvalidInput",
            "description": "<p>One of inputs was null or invalid</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "NotFound",
            "description": "<p>Case with specified ID was not found</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 200 OK\n{\n  \"status\":\"failure\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 200 OK\n{\n \"status\":\"notFound\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "./Controllers/CaseController.cs",
    "groupTitle": "Case"
  },
  {
    "type": "post",
    "url": "/Home",
    "title": "Create",
    "version": "0.1.0",
    "name": "Create",
    "group": "Home",
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "Title",
            "description": "<p>Rule title</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "Description",
            "description": "<p>Rule details</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "status",
            "description": "<p>Rule was created</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n      {\n      \"status\":\"created\"\n      }",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "InvalidInput",
            "description": "<p>One of inputs was null or invalid</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 200 OK\n{\n  \"status\":\"failure\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "./Controllers/HomeController.cs",
    "groupTitle": "Home"
  },
  {
    "type": "delete",
    "url": "/Home?RuleID",
    "title": "Delete",
    "version": "0.1.0",
    "name": "Delete",
    "group": "Home",
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "GUID",
            "optional": false,
            "field": "RuleID",
            "description": "<p>Rule identifier</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "status",
            "description": "<p>Rule was deleted</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n      {\n      \"status\":\"removed\"\n      }",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "NotFound",
            "description": "<p>Rule with specified ID was not found</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 200 OK\n{\n \"status\":\"notFound\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "./Controllers/HomeController.cs",
    "groupTitle": "Home"
  },
  {
    "type": "get",
    "url": "/Home",
    "title": "List",
    "version": "0.1.0",
    "name": "List",
    "group": "Home",
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "Array",
            "optional": false,
            "field": "rules",
            "description": "<p>List of all rules</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n[\n   { \n   \"RuleID\":\"4ba83f3c-4ea4-4da4-9c06-e986a8273800\",\n   \"Name\":\"Rule1\",\n   \"Description\":\"Rule 1 desc\"\n   }\n]",
          "type": "json"
        }
      ]
    },
    "filename": "./Controllers/HomeController.cs",
    "groupTitle": "Home"
  },
  {
    "type": "get",
    "url": "/Home?RuleID",
    "title": "Read",
    "version": "0.1.0",
    "name": "Read",
    "group": "Home",
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "GUID",
            "optional": false,
            "field": "RuleID",
            "description": "<p>Rule identifier</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "RuleID",
            "description": "<p>Rule identifier</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "Title",
            "description": "<p>Rule title</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "Description",
            "description": "<p>Rule details</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n      {\n      \"RuleID\":\"4ba83f3c-4ea4-4da4-9c06-e986a8273800\",\n      \"Title\":\"ExampleRule\",\n      \"Description\":\"Restrict something\",\n      }",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "NotFound",
            "description": "<p>Given ID does not appeal to any of rules</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 200 OK\n{\n  \"status\":\"notFound\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "./Controllers/HomeController.cs",
    "groupTitle": "Home"
  },
  {
    "type": "put",
    "url": "/Home?RuleID",
    "title": "Update",
    "version": "0.1.0",
    "name": "Update",
    "group": "Home",
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "GUID",
            "optional": false,
            "field": "RuleID",
            "description": "<p>Rule identifier</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "Title",
            "description": "<p>Rule title</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "Description",
            "description": "<p>Rule details</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "status",
            "description": "<p>Rule was updated</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n      {\n      \"status\":\"updated\"\n      }",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "InvalidInput",
            "description": "<p>One of inputs was null or invalid</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "NotFound",
            "description": "<p>Rule with specified ID was not found</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 200 OK\n{\n  \"status\":\"failure\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 200 OK\n{\n \"status\":\"notFound\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "./Controllers/HomeController.cs",
    "groupTitle": "Home"
  },
  {
    "type": "post",
    "url": "/Room",
    "title": "Create",
    "version": "0.1.0",
    "name": "Create",
    "group": "Room",
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "Number",
            "optional": false,
            "field": "Number",
            "description": "<p>Number of room</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "status",
            "description": "<p>Room was created</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n      {\n      \"status\":\"created\"\n      }",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "InvalidInput",
            "description": "<p>One of inputs was null or invalid</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 200 OK\n{\n  \"status\":\"failure\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "./Controllers/RoomController.cs",
    "groupTitle": "Room"
  },
  {
    "type": "delete",
    "url": "/Room?RoomID",
    "title": "Delete",
    "version": "0.1.0",
    "name": "Delete",
    "group": "Room",
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "GUID",
            "optional": false,
            "field": "RoomID",
            "description": "<p>Room identifier</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "status",
            "description": "<p>Room was deleted</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n      {\n      \"status\":\"removed\"\n      }",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "NotFound",
            "description": "<p>Room with specified ID was not found</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 200 OK\n{\n \"status\":\"notFound\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "./Controllers/RoomController.cs",
    "groupTitle": "Room"
  },
  {
    "type": "get",
    "url": "/Room",
    "title": "List",
    "version": "0.1.0",
    "name": "List",
    "group": "Room",
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "Array",
            "optional": false,
            "field": "rules",
            "description": "<p>List of all rooms</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n[\n   { \n   \"RoomID\":\"4ba83f3c-4ea4-4da4-9c06-e986a8273800\",\n   \"GuestFirstName\":\"Marco\",\n   \"GuestLastName\":\"Polo\",\n   \"Number\":9,\n   \"Occupied\":false\n   }\n]",
          "type": "json"
        }
      ]
    },
    "filename": "./Controllers/RoomController.cs",
    "groupTitle": "Room"
  },
  {
    "type": "get",
    "url": "/Room?RoomID",
    "title": "Read",
    "version": "0.1.0",
    "name": "Read",
    "group": "Room",
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "GUID",
            "optional": false,
            "field": "RoomID",
            "description": "<p>Room identifier</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "RoomID",
            "description": "<p>Room identifier</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "GuestFirstName",
            "description": "<p>If room is occupied here will be name of client</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "GuestLastName",
            "description": "<p>If room is occupied here will be surname of client</p>"
          },
          {
            "group": "Success 200",
            "type": "Boolean",
            "optional": false,
            "field": "Occupied",
            "description": "<p>Is room free</p>"
          },
          {
            "group": "Success 200",
            "type": "Number",
            "optional": false,
            "field": "Number",
            "description": "<p>Number of room</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n     { \n   \"RoomID\":\"4ba83f3c-4ea4-4da4-9c06-e986a8273800\",\n   \"GuestFirstName\":\"Marco\",\n   \"GuestLastName\":\"Polo\",\n   \"Number\":9,\n   \"Occupied\":false\n   }",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "NotFound",
            "description": "<p>Given ID does not appeal to any of rooms</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 200 OK\n{\n  \"status\":\"notFound\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "./Controllers/RoomController.cs",
    "groupTitle": "Room"
  },
  {
    "type": "put",
    "url": "/Room?RoomID",
    "title": "Update",
    "version": "0.1.0",
    "name": "Update",
    "group": "Room",
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "GUID",
            "optional": false,
            "field": "RoomID",
            "description": "<p>Room identifier</p>"
          },
          {
            "group": "Parameter",
            "type": "Number",
            "optional": false,
            "field": "Number",
            "description": "<p>Room number</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "status",
            "description": "<p>Room was updated</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n      {\n      \"status\":\"updated\"\n      }",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "InvalidInput",
            "description": "<p>One of inputs was null or invalid</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "NotFound",
            "description": "<p>Room with specified ID was not found</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 200 OK\n{\n  \"status\":\"failure\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 200 OK\n{\n \"status\":\"notFound\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "./Controllers/RoomController.cs",
    "groupTitle": "Room"
  },
  {
    "type": "post",
    "url": "/Task",
    "title": "Create",
    "version": "0.1.0",
    "name": "Create",
    "group": "Task",
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "status",
            "description": "<p>task was created</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n      {\n      \"status\":\"created\"\n      }",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "InvalidInput",
            "description": "<p>One of inputs was null or invalid</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 200 OK\n{\n  \"status\":\"failure\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "./Controllers/TaskController.cs",
    "groupTitle": "Task"
  },
  {
    "type": "delete",
    "url": "/Task/TaskID",
    "title": "Delete",
    "version": "0.1.0",
    "name": "Delete",
    "group": "Task",
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "GUID",
            "optional": false,
            "field": "TaskID",
            "description": "<p>Task identifier</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "status",
            "description": "<p>Task was deleted</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n      {\n      \"status\":\"removed\"\n      }",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "NotFound",
            "description": "<p>Task with specified ID was not found</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 200 OK\n{\n \"status\":\"notFound\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "./Controllers/TaskController.cs",
    "groupTitle": "Task"
  },
  {
    "type": "get",
    "url": "/Task",
    "title": "List",
    "version": "0.1.0",
    "name": "List",
    "group": "Task",
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "Array",
            "optional": false,
            "field": "tasks",
            "description": "<p>List of all existing tasks</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n  [\n      {\n      \"TaskID\":\"4ba83f3c-4ea4-4da4-9c06-e986a8273800\",\n      \"Describe\":\"Describtion of task\",\n      \"RoomID\":\"5ba83f3c-4ea4-4da4-9c06-e986a8273800\",\n      \"Room\":\"Connected room\"\n      }\n  ]",
          "type": "json"
        }
      ]
    },
    "filename": "./Controllers/TaskController.cs",
    "groupTitle": "Task"
  },
  {
    "type": "get",
    "url": "/Task/TaskID",
    "title": "Read",
    "version": "0.1.0",
    "name": "Read",
    "group": "Task",
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "GUID",
            "optional": false,
            "field": "TaskID",
            "description": "<p>Task identifier</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "TaskID",
            "description": "<p>Task identifier</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "Description",
            "description": "<p>of task</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "RoomID",
            "description": "<p>Room identifier</p>"
          },
          {
            "group": "Success 200",
            "type": "Room",
            "optional": false,
            "field": "Room",
            "description": ""
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n      {\n      \"TaskID\":\"4ba83f3c-4ea4-4da4-9c06-e986a8273800\",\n      \"Describe\":\"Describtion of task\",\n      \"RoomID\":\"5ba83f3c-4ea4-4da4-9c06-e986a8273800\",\n      \"Room\":\"Connected room\"\n      }",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "NotFound",
            "description": "<p>Given ID does not appeal to any of tasks</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 200 OK\n{\n  \"status\":\"notFound\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "./Controllers/TaskController.cs",
    "groupTitle": "Task"
  },
  {
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "optional": false,
            "field": "varname1",
            "description": "<p>No type.</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "varname2",
            "description": "<p>With type.</p>"
          }
        ]
      }
    },
    "type": "",
    "url": "",
    "version": "0.0.0",
    "filename": "./wwwroot/doc/main.js",
    "group": "_home_magdalena_RiderProjects_HMS_HotelManagementSystem_HotelManagementSystem_wwwroot_doc_main_js",
    "groupTitle": "_home_magdalena_RiderProjects_HMS_HotelManagementSystem_HotelManagementSystem_wwwroot_doc_main_js",
    "name": ""
  },
  {
    "type": "put",
    "url": "/task?TaskID",
    "title": "Update",
    "version": "0.1.0",
    "name": "Update",
    "group": "task",
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "status",
            "description": "<p>task was updated</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n      {\n      \"status\":\"updated\"\n      }",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "InvalidInput",
            "description": "<p>One of inputs was null or invalid</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "NotFound",
            "description": "<p>task with specified ID was not found</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 200 OK  \n{\n  \"status\":\"failure\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 200 OK\n{\n \"status\":\"notFound\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "./Controllers/TaskController.cs",
    "groupTitle": "task"
  }
] });
