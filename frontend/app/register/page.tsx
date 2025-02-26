"use client"
import React, { FormEventHandler, useState } from 'react'

const Register = () => {
  const [email, setEmail] = useState("")
  const [password, setPassword] = useState("")
  const fetchPost = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    const res = await fetch("http://localhost:8080/api/User/registration", {
      method: "POST",
      headers: {
        "Access-Control-Allow-Origin": "*",
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        login: email,
        password: password
      })
    })
    if(await res.text() === "Created"){
      window.location.href = "/"
    }
  }
  return (
    <div>
      <form onSubmit={fetchPost}>
        <label htmlFor='email'>Email:</label><br />
        <input name='email' type='email' required placeholder='example@gmail.com' onChange={e => setEmail(e.target.value)} /><br />
        <label htmlFor='password'>Password:</label><br />
        <input name='password' type='password' required placeholder='****' onChange={e => setPassword(e.target.value)} /><br />
        <input type='submit' value='Sign Up' />
      </form>
    </div>
  )
}

export default Register