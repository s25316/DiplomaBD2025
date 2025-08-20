'use client'

import { OuterContainer } from "@/app/components/layout/PageContainers";
import { useSession } from "next-auth/react";
import { useParams, useRouter } from "next/navigation";
import { useEffect, useState } from "react";

const Recruite = () => {
    const { offerId } = useParams() as {
        id: string;
        branchId: string;
        offerId: string;
    };

    const { data: session } = useSession();
    const router = useRouter();

    const [file, setFile] = useState<File | null>(null)
    const [description, setDescription] = useState<string>("")

    const backUrl = process.env.NEXT_PUBLIC_API_URL

    useEffect(() => {
        const check = async () => {
            if (session?.user?.token) {
                const res = await fetch(`${backUrl}/api/User`, {
                    method: 'GET',
                    headers: {
                        'Authorization': `Bearer ${session.user.token}`,
                    }
                })
                const json = await res.json()
                if(!json.personPerspective.isIndividual){
                    return (<div>Unauthorized</div>)
                }
            }else{
                return (<div>Unauthorized</div>)
            }
        }

        check();
    }, [])

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

        fetch(`${backUrl}/api/User/recruitments/${offerId}`, {
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
        <OuterContainer className="max-w-xl mx-auto p-4">
            <form className="flex flex-col space-y-4" onSubmit={handleSubmit}>
                <label htmlFor='fileInput'>CV:</label>
                <input name="fileInput" type='file' accept="application/pdf" required onChange={handleFileChange} />
                <label htmlFor="description">Description:</label>
                <textarea className='global-field-style' name="description" rows={4} onChange={x => setDescription(x.target.value)} />
                <button className="inline-block bg-green-600  p-2 rounded-md hover:bg-green-700 transition-colors" type="submit">Apply</button>
            </form>
        </OuterContainer>
    )
}

export default Recruite