"use client"

import React, { useState } from "react"
import { OuterContainer } from "../components/layout/PageContainers"
import { useRouter } from "next/navigation"

const ResetPassword = () => {
    const [email, setEmail] = useState("")

    const router = useRouter();

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault()

        const res = await fetch('http://localhost:8080/api/User/password/initiate', {
            method: "PUT",
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                login: email
            })
        })

        if(res.ok){
            alert("Reset password link sent to email")
            router.push("/")
        }
    }

    return (
        <OuterContainer className='w-[500px] p-8'>
            <form className='flex flex-col space-y-4' onSubmit={handleSubmit}>
                <div className="space-y-2">
                    <label htmlFor='email' className="block text-sm font-medium">Email:</label>
                    <input type='email' name='email' required
                        className="w-full p-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                        onChange={e => setEmail(e.target.value)} />
                </div>
                <button type='submit'
                    className="w-full bg-blue-500 text-white p-2 rounded-md hover:bg-blue-600 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2 disabled:opacity-50 disabled:cursor-not-allowed">
                    Reset
                </button>
            </form>
        </OuterContainer>
    )
}

export default ResetPassword