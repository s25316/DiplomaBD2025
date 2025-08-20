'use client'

import { OuterContainer } from "@/app/components/layout/PageContainers"
import { useParams, useRouter } from "next/navigation";
import React, { useState } from "react"

const ResetPasswordLink = () => {
    const { userId, urlSegment } = useParams() as { userId: string; urlSegment: string };

    const [newPassword, setNewPassword] = useState("");
    const [newPasswordConfirm, setNewPasswordConfirm] = useState("");

    const [error, setError] = useState("");

    const router = useRouter()

    const backUrl = process.env.NEXT_PUBLIC_API_URL

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError("");
        if (newPassword !== newPasswordConfirm) {
            setError("Passwords doesn't match")
            return;
        }

        const res = await fetch(`${backUrl}/api/User/password/${userId}/${urlSegment}`, {
            method: "PUT",
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                newPassword
            })
        })

        if (res.ok) {
            alert("Your password has been changed")
            router.push("/login")
        } else {
            setError("Something went wrong, please try again later")
        }
    }

    return (
        <OuterContainer className="w-[500px] p-8">
            <h1 className="text-2xl font-bold mb-6 text-center">Log In</h1>
            <form className='flex flex-col space-y-4' onSubmit={handleSubmit}>
                <div className="space-y-2">
                    <label htmlFor='new-password' className="block text-sm font-medium">
                        New Password:
                    </label>
                    <input
                        name='new-password'
                        type="password"
                        className="w-full p-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                        onChange={e => setNewPassword(e.target.value)}
                        required
                    />
                </div>
                <div className="space-y-2">
                    <label htmlFor='new-password-confirm' className="block text-sm font-medium">
                        New Password Confirm:
                    </label>
                    <input
                        name='new-password-confirm'
                        type="password"
                        className="w-full p-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                        onChange={e => setNewPasswordConfirm(e.target.value)}
                        required
                    />
                </div>

                {error && (
                    <div className="text-red-500 text-sm text-center">
                        {error}
                    </div>
                )}

                <button
                    type='submit'
                    className="w-full bg-blue-500 text-white p-2 rounded-md hover:bg-blue-600 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2 disabled:opacity-50 disabled:cursor-not-allowed"
                >
                    Set new password
                </button>
            </form>
        </OuterContainer>
    )
}

export default ResetPasswordLink