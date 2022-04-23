// Next.js API route support: https://nextjs.org/docs/api-routes/introduction

export default function handler(req, res) {
    res.status(200).json([
        {id: 1, cliente: 'Carlos Torres', cin: '1234567', estado: 'Pendiente', montTotal:  12500000, saldo: 12500000, cantidad: 1}
    ])
  }
  