'use client'

import { OuterContainer } from "@/app/components/layout/PageContainers";
import { useSession } from "next-auth/react";
import { useParams, useRouter } from "next/navigation";
import { useState } from "react";

const Recruite = () => {
    const { id, branchId, offerId } = useParams() as {
        id: string;
        branchId: string;
        offerId: string;
    };

    const { data: session } = useSession();
    const router = useRouter();

    const [file, setFile] = useState<File | null>(null)
    const [description, setDescription] = useState<string>("")

    const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const funcFile = event.target.files?.[0];

        if (funcFile && funcFile.type === "application/pdf") {
            setFile(funcFile);
        } else {
            setFile(null);
            alert("Please select a valid PDF file.");
        }
    };

    const handleSubmit = (event: React.ChangeEvent<HTMLFormElement>) => {
        event.preventDefault()

        if (!session?.user?.token) return;

        if (file === null) {
            alert("Please select a valid PDF file.");
            return;
        }

        const formData = new FormData();
        formData.append("File", file)
        formData.append("Description", description)

        fetch(`http://localhost:8080/api/User/recruitments/${offerId}`, {
            method: 'POST',
            headers: {
                'Authorization': `Bearer ${session.user.token}`,
            },
            body: formData,
        })
            .then(res => {
                if (res.ok) {
                    alert('Applied successfully!');
                    router.back();
                }
            })
            .catch(() => {
                alert('Failed to apply.')
            });
    }

    if (!session?.user?.token) return <div className="text-center p-4">Unauthorized</div>;

    return (
        <OuterContainer>
            <form className="flex flex-col space-y-4" onSubmit={handleSubmit}>
                <label htmlFor='fileInput'>CV:</label>
                <input name="fileInput" type='file' accept="application/pdf" required onChange={handleFileChange} />
                <label htmlFor="description">Description:</label>
                <input name="description" type="text" onChange={x => setDescription(x.target.value)} />
                <button className="inline-block bg-green-600  p-2 rounded-md hover:bg-green-700 transition-colors" type="submit">Apply</button>
            </form>
        </OuterContainer>
    )
}

export default Recruite