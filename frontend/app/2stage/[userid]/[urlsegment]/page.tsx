'use client'

import React, { FormEvent, useState } from 'react'
import { usePathname } from 'next/navigation'
import { useRouter } from 'next/navigation'

const TwoFactorAuth = () => {
    const [code, setCode] = useState('')
    const pathname = usePathname()
    const pathnames = pathname.split("/")
    const router = useRouter()

    const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
        e.preventDefault()
        const res = await fetch(`http://localhost:8080/api/User/handPart/${pathnames.at(2)}/${pathnames.at(3)}`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                'Access-Control-Allow-Origin': '*',
            },
            body: JSON.stringify({ code }),
        })

        if (res.ok) {
            router.push("/api/auth/signin")
        }
        else {
            alert("Invalid code")
        }
    }

    return (
        <div>
            <form onSubmit={handleSubmit}>
                <input id="code" type="number" placeholder='Code' onChange={(e) => setCode(e.target.value)} required />
                <button type='submit'>Submit</button>
            </form>
        </div>
    )
}

export default TwoFactorAuth