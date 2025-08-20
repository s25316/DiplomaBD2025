'use client'

import React, { FormEvent, useState } from 'react'
import { usePathname } from 'next/navigation'
import { useRouter } from 'next/navigation'
import { OuterContainer } from '@/app/components/layout/PageContainers'

const TwoFactorAuth = () => {
    const [code, setCode] = useState('')
    const pathname = usePathname()
    const pathnames = pathname.split("/")
    const router = useRouter()
    const backUrl = process.env.NEXT_PUBLIC_API_URL

    const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
        e.preventDefault()
        const res = await fetch(`${backUrl}/api/User/handPart/${pathnames.at(2)}/${pathnames.at(3)}`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                'Access-Control-Allow-Origin': '*',
            },
            body: JSON.stringify({ code }),
        })

        if (res.ok) {
            router.push("/login")
        }
        else {
            alert("Invalid code")
        }
    }

    return (
        <OuterContainer className="w-[500px] p-8">
            <h1 className="text-2xl font-bold mb-6 text-center">2FA</h1>
            <form onSubmit={handleSubmit} className='flex flex-col space-y-4'>
                <input className="w-full p-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500" id="code" type="number" placeholder='Code' onChange={(e) => setCode(e.target.value)} required />
                <button className="w-full bg-blue-500 text-white p-2 rounded-md hover:bg-blue-600 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2 disabled:opacity-50 disabled:cursor-not-allowed" type='submit'>Submit</button>
            </form>
        </OuterContainer>
    )
}

export default TwoFactorAuth