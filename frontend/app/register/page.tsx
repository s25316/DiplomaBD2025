"use client"
import { redirect } from 'next/navigation'
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
        email: email,
        password: password
      })
    })
    if(res.ok){
      redirect("/api/auth/signin")
    }
    else{
      alert("Registration faild")
    }
  }
  return (
    <div>
      <form onSubmit={fetchPost} className="flex flex-col gap-4">
        <label htmlFor='email'>Email:</label>
        <input name='email' type='email' required placeholder='example@gmail.com' onChange={e => setEmail(e.target.value)} />
        <label htmlFor='password'>Password:</label>
        <input name='password' type='password' required placeholder='****' onChange={e => setPassword(e.target.value)} />
        <button type='submit'>Sign Up</button>
      </form>
    </div>
  )
}

export default Register