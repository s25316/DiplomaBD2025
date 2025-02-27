"use client";
import { useSession } from "next-auth/react";
import { useRouter } from "next/navigation";
import React from "react";

const Profile = () => {
  const { data: session } = useSession();
  const router = useRouter();

  return (
    <div>
      <h1>Profile</h1>
      {session?.user ? (
        <>
          <p>Logged in as: {session.user.token}</p>
          <button onClick={() => router.push("/profile/createCompany")}>
            Create Company
          </button>
        </>
      ) : (
        <p>You need to be logged in.</p>
      )}
    </div>
  );
};

export default Profile;
