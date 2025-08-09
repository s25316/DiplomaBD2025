'use client'

import React, { useState } from 'react'
import { OuterContainer } from "@/app/components/layout/PageContainers"
import { useSession } from 'next-auth/react'
import { useRouter } from 'next/navigation'
import CancelButton from '@/app/components/buttons/CancelButton'

const PasswordChange = () => {
    const [oldPassword, setOldPassword] = useState("");
    const [newPassword, setNewPassword] = useState("");
    const [newPasswordConfirm, setNewPasswordConfirm] = useState("");

    const [errorPasswordNotMatch, setErrorPasswordNotMatch] = useState("");
    const [errorOldPassword, setErrorOldPassword] = useState("");

    const { data: session, status } = useSession()

    const router = useRouter();

    if (status === "loading") {
        return (<p className='flex justify-center'>Loading page</p>)
    }

    if (status === "unauthenticated") {
        return (<p className='text-red-600 flex justify-center'>Unauthenticated</p>)
    }

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setErrorPasswordNotMatch("")
        setErrorOldPassword("")
        if (newPassword !== newPasswordConfirm) {
            setErrorPasswordNotMatch("New password doesn't match");
            return
        }

        const res = await fetch('http://localhost:8080/api/User/password', {
            method: "PUT",
            headers: {
                Authorization: `Bearer ${session?.user.token}`,
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                oldPassword: oldPassword,
                newPassword: newPassword
            })
        })

        if (res.status === 400) {
            setErrorOldPassword("Old password does not match")
            return
        }
        if(res.ok){
            router.push("/profile")
        }
    }

    return (
        <OuterContainer className='w-[500px] p-8'>
            <h1 className="text-2xl font-bold mb-6 text-center">Change Password</h1>
            <form className='flex flex-col space-y-4' onSubmit={handleSubmit}>
                <div className="space-y-2">
                    <label htmlFor='old-password' className="block text-sm font-medium">Old password:</label>
                    <input type='text' name='old-password' required
                        className="w-full p-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                        onChange={e => setOldPassword(e.target.value)} />
                    {errorOldPassword && (
                        <div className='text-red-500'>
                            {errorOldPassword}
                        </div>
                    )}
                </div>
                <div className="space-y-2">
                    <label htmlFor='new-password' className="block text-sm font-medium">New password:</label>
                    <input type='password' name='new-password' required
                        className="w-full p-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                        onChange={e => setNewPassword(e.target.value)} />
                </div>
                <div className="space-y-2">
                    <label htmlFor='new-password-confirm' className="block text-sm font-medium">Confirm password:</label>
                    <input type='password' name='new-password-confirm' required
                        className="w-full p-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                        onChange={e => setNewPasswordConfirm(e.target.value)} />
                    {errorPasswordNotMatch && (
                        <div className='text-red-500'>
                            {errorPasswordNotMatch}
                        </div>
                    )}
                </div>
                <button type='submit'
                    className="w-full bg-blue-500 text-white p-2 rounded-md hover:bg-blue-600 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2 disabled:opacity-50 disabled:cursor-not-allowed">
                    Change</button>
                <CancelButton/>
            </form>
        </OuterContainer>
    )
}

export default PasswordChange