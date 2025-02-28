import React from "react";
import CreateCompanyButton from "@/app/components/buttons/CreateCompanyButton"
import { getServerSession } from "next-auth";
import { authOptions } from "../api/auth/[...nextauth]/route";
import { redirect } from "next/navigation";

const Profile = async () => {
  const session = await getServerSession(authOptions);

  if (!session?.user) {
    redirect("/api/auth/signin")
  }

  return (
    <div>
      <h1>Profile</h1>
      <p>Logged in as: {session.user.token}</p>
      <CreateCompanyButton />
    </div>
  );
};

export default Profile;
