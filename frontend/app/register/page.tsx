"use client"
import { redirect } from 'next/navigation'
import React, { useState } from 'react'
import { OuterContainer } from '../components/layout/PageContainers'

const Register = () => {
  const [email, setEmail] = useState("")
  const [password, setPassword] = useState("")

  const backUrl = process.env.NEXT_PUBLIC_API_URL

  const fetchPost = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    const res = await fetch(`${backUrl}/api/User/registration`, {
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
    <OuterContainer className='w-[500px]'>
      <form onSubmit={fetchPost} className="flex flex-col gap-4">
        <label htmlFor='email'>Email:</label>
        <input className="w-full p-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500" name='email' type='email' required placeholder='example@gmail.com' onChange={e => setEmail(e.target.value)} />
        <label htmlFor='password'>Password:</label>
        <input className="w-full p-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500" name='password' type='password' required placeholder='****' onChange={e => setPassword(e.target.value)} />
        <button className="inline-block bg-green-600  p-2 rounded-md hover:bg-green-700 transition-colors" type='submit'>Sign Up</button>
      </form>
    </OuterContainer>
  )
}

export default Register